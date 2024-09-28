using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils.Logger;

namespace Utils.StateMachines
{
	public class StateMachine<K> : IStateMachine<K>, IDisposable
	{
		public IState<K> ActiveState { get; protected set; }
		public IStateMachine<K>.SwitchInfo ActiveSwitch { get; private set; }
		protected readonly Dictionary<K, IState<K>> States;
		public event StateChangeDelegate<K> OnStateChange;
		public event ExceptionHandlerDelegate OnException;

		public StateMachine(IEnumerable<IState<K>> states)
		{
			States = states.ToDictionary(state => state.Key, state => state);
		}

		public bool ContainsState(K key)
		{
			return States.ContainsKey(key);
		}
		public void AddOrReplaceState(IState<K> state)
		{
			States[state.Key] = state;
		}

		public virtual async Task SwitchState(IStateData<K> data)
		{
			K key = data.Key;
			if (States.TryGetValue(key, out IState<K> state))
				await SwitchState(state, data);
		}

		public virtual Task SwitchState(IState<K> state)
		{
			return SwitchState(state, null);
		}

		public virtual async Task SwitchState(K key)
		{
			if (States.TryGetValue(key, out IState<K> state))
				await SwitchState(state, null);
		}

		public virtual async Task ExitActiveState()
		{
			if (ActiveState == null) return;

			try
			{
				await ActiveState.Exit();
				await ActiveState.Cleanup();
			}
			catch (Exception e)
			{
				OnException(e);
			}

			ActiveState = null;
		}

		protected void StateChanged(IState<K> state) => OnStateChange(ActiveState, state);
		protected void ExceptionCaught(Exception e) => OnException(e);

		protected virtual async Task SwitchState(IState<K> state, IStateData<K> data)
		{
			IState<K> oldState = ActiveState;
			if (ActiveSwitch.IsSwitching)
			{
				Exception e = new Exception("StateMachine is already switching states!\n" +
					$"Called Switch: {nameof(SwitchState)}({state}, {data})\n" +
					$"Active Switch: [{ActiveSwitch.oldState}] To [{ActiveSwitch.newState}] with data [{ActiveSwitch.newStateData}]");

				e.LogException();
				throw e;
			}

			ActiveSwitch = new IStateMachine<K>.SwitchInfo(ActiveState, state, data);
			try
			{
				if (state == oldState)
				{
					await oldState.Reload(data);
					return;
				}

				await state.Preload(data);

				if (oldState != null)
					await oldState.Exit();

				ActiveState = state;
				await state.Enter(this);

				if (oldState != null)
					await oldState?.Cleanup();

				OnStateChange?.Invoke(oldState, state);
			}
			catch (Exception e)
			{
				OnException?.Invoke(e);
			}
			finally
			{
				ActiveSwitch = default;
			}
		}

		public virtual void Dispose()
		{
			foreach (IState<K> state in States.Values)
			{
				if (state is IDisposable disp)
					disp.Dispose();
			}
		}
	}
}
