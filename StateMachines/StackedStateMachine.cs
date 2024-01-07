using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utils.Collections;
using Utils.StateMachines;

namespace Utils.StateMachines
{
	public interface IStackableState
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
				await ActiveState.Exit();
				await ActiveState.Cleanup();
				if (ActiveState is IStackableState stackable)
					await stackable.OnDestacked(stack.Count);
			}
			catch (Exception e)
			{
				ExceptionCaught(e);
			}

			await DestackRange(0, stack.Count);
		}

		public async Task BackState()
		{
			if (stack.TryPop(out IState<K> prevState))
			{
				await SwitchState(prevState, null, false);
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

		public async Task SwitchState(IState<K> state, IStateData<K> data = null, bool doStack = true)
		{
			if (state == null)
			{
				await ExitActiveState();
				return;
			}

			IState<K> oldState = ActiveState;
			bool hadState = oldState != null;
			if (hadState && oldState.Key.Equals(state.Key))
			{
				try
				{
					await oldState.Reload(data);
				}
				catch (Exception e)
				{
					ExceptionCaught(e);
				}

				return;
			}

			bool isStackable = true;
			if (oldState is not IStackableState stackable)
			{
				isStackable = false;
				stackable = null;
			}

			try
			{
				await state.Preload(data);

				if (doStack && oldState != null)
					stack.Add(oldState);

				if (hadState)
					await oldState.Exit();

				if (doStack && isStackable) 
					await stackable.OnStacked(stack.Count - 1);

				ActiveState = state;
				await state.Enter(this);

				if (!doStack && isStackable) 
					await stackable?.OnDestacked(stack.Count);

				if (hadState)
					await oldState.Cleanup();
			}
			catch (Exception e)
			{
				ExceptionCaught(e);
			}
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
			await (state as IStackableState)?.OnDestacked(stack.Count);
			await state.Cleanup();
		}
	}
}
