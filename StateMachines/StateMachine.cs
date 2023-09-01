using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utils.StateMachines
{
	public class StateMachine<K> : IStateMachine<K>
	{
		public IState<K> ActiveState { get; protected set; }
		IState IStateMachine.ActiveState => ActiveState;
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

		public async Task ExitActiveState()
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

		private async Task SwitchState(IState<K> state, IStateData<K> data)
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

				OnStateChange(ActiveState, state);
			}
			catch (Exception e)
			{
				OnException(e);
			}
		}
	}
}
