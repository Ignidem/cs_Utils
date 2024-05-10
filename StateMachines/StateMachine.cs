using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utils.StateMachines
{
	public class StateMachine<K> : IStateMachine<K>, IDisposable
	{
		public IState<K> ActiveState { get; protected set; }
		protected Dictionary<K, IState<K>> States;
		public event StateChangeDelegate<K> OnStateChange;
		public event ExceptionHandlerDelegate OnException;

		public StateMachine(IEnumerable<IState<K>> states)
		{
			States = states.ToDictionary(state => state.Key, state => state);
		}

		public async Task SwitchState(IStateData<K> data)
		{
			K key = data.Key;
			if (States.TryGetValue(key, out IState<K> state))
				await SwitchState(state, data);
		}

		public async Task SwitchState(K key)
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
			if (state == ActiveState) return;

			try
			{
				await state.Preload(data);

				IState<K> oldState = ActiveState;
				if (oldState != null)
					await oldState.Exit();

				ActiveState = state;
				await ActiveState.Enter(this);

				if (oldState != null)
					await oldState?.Cleanup();

				OnStateChange(oldState, state);
			}
			catch (Exception e)
			{
				OnException(e);
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
