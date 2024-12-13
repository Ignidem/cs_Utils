using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utils.Collections;
using Utils.Logger;
using Utils.StateMachines;

namespace Utils.StateMachines
{
	public interface IStackableState<K> : IStackableState, IState<K> { }
	public interface IStackableState : IState
	{
		/// <summary>
		/// State has Exited and was stacked.
		/// </summary>
		/// <param name="index">The index at which the state was stacked.</param>
		/// <returns></returns>
		Task OnStacked(int index);

		/// <summary>
		/// State has exited but will not be stacked.
		/// </summary>
		/// <param name="index">The index the state was stacked at.</param>
		/// <returns></returns>
		Task OnDestacked(int index);
	}

	public class StackedStateMachine<K> : StateMachine<K>
	{
		private readonly List<IState<K>> stack = new List<IState<K>>();

		public StackedStateMachine(IEnumerable<IState<K>> states) : base(states) { }

		public async Task WipeStack()
		{
			try
			{
				IState<K> activeState = ActiveState;
				if (activeState != null)
				{
					await activeState.Exit();
					await activeState.Cleanup();
					if (activeState is IStackableState stackable)
						await stackable.OnDestacked(stack.Count);
				}
			}
			catch (Exception e)
			{
				ExceptionCaught(e);
			}

			await DestackRange(0, stack.Count);
		}

		public async Task BackState(IStateData<K> data = null)
		{
			if (stack.TryPop(out IState<K> prevState))
			{
				await SwitchState(prevState, data, false);
				return;
			}

			try
			{
				await ExitState(ActiveState);
			}
			catch (Exception e)
			{
				ExceptionCaught(e);
			}
		}

		public Task SwitchState(IStateData<K> data, bool doStack = true)
		{
			K key = data.Key;
			if (!States.TryGetValue(key, out IState<K> state))
				return Task.CompletedTask;

			return SwitchState(state, data, doStack);
		}

		public async Task SwitchState(IState<K> state, IStateData<K> data, bool doStack = true)
		{

			if (CheckPendingTransition())
				return;

			if (state == null)
			{
				await ExitActiveState();
				return;
			}

			try
			{
				IState<K> exitingState = ActiveState;
				if (exitingState != null && exitingState.Key.Equals(state.Key))
				{
					await exitingState.Reload(data);
					return;
				}

				lastTransition = new TransitionInfo<K>(exitingState, state, data);
				await (transitionTask = HandleTransition(state, data, doStack));
			}
			catch (Exception e)
			{
				ExceptionCaught(e);
			}
		}

		private async Task HandleTransition(IState<K> state, IStateData<K> data, bool doStack)
		{
			IState<K> exitingState = ActiveState;
			//We are stacking the previous state to enable returning to it.
			if (exitingState is not IStackableState<K> stackable)
				stackable = null;

			TransitionChanged(TransitionType.Preload);
			await state.Preload(data);

			if (doStack && stackable != null)
				stack.Add(stackable);

			if (exitingState != null)
			{
				TransitionChanged(TransitionType.Exit);
				await exitingState.Exit();
			}

			if (doStack && stackable != null)
				await stackable.OnStacked(stack.Count - 1);

			TransitionChanged(TransitionType.Enter);
			await state.Enter(this);
			lastTransition.OnEnter();

			if (exitingState != null)
			{
				TransitionChanged(TransitionType.Cleanup);
				await exitingState.Cleanup();
			}

			if (!doStack && stackable != null)
				await stackable.OnDestacked(stack.Count);
		}

		public async Task JumpState(K key)
		{
			int index = stack.IndexOf(d => d.Key.Equals(key));
			if (index < 0) return;
			index++;
			await DestackRange(index, stack.Count - index);
			if (!stack.TryPop(out IState<K> state)) return;
			await SwitchState(state, null, false);
		}

		private async Task DestackRange(int index, int count)
		{
			for (int i = index + (count - 1); i >= index; i--)
			{
				try
				{
					IState<K> state = stack[i];
					await ExitState(state);
					stack.RemoveAt(i);
				}
				catch (Exception e)
				{
					ExceptionCaught(e);
				}
			}
		}

		private async Task ExitState(IState<K> state)
		{
			await state.Exit();
			await state.Cleanup();
			await (state as IStackableState)?.OnDestacked(stack.Count);
		}
	}
}
