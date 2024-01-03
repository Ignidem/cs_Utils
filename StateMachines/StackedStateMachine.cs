using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utils.Collections;
using Utils.StateMachines;

namespace Utils.StateMachines
{
	public interface IStackableState
	{
		Task OnStacked(int index);
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
				await SwitchState(prevState, false);
				return;
			}

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
		}

		public async Task SwitchState(IState<K> state, bool doStack = true)
		{
			try
			{
				IState<K> oldState = ActiveState;
				IStackableState stackable = oldState as IStackableState;
				await state.Preload(null);
				if (doStack) 
				{
					stack.Add(oldState);
					await stackable?.OnStacked(stack.Count - 1);
				}

				await oldState.Exit();
				ActiveState = state;
				await state.Enter(this);
				await oldState.Cleanup();
				if (!doStack)
				{
					await stackable?.OnDestacked(stack.Count);
				}
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
			if (!stack.TryPop(out var data)) return;
			await SwitchState(data, false);
		}

		private async Task DestackRange(int index, int count)
		{
			for (int i = index + (count - 1); i >= index; i--)
			{
				try
				{
					IState<K> state = stack[i];
					await state.Exit();
					await state.Cleanup();
					stack.RemoveAt(i);
				}
				catch (Exception e)
				{
					ExceptionCaught(e);
				}
			}
		}
	}
}
