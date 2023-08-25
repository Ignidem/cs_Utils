using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Utils.StateMachines
{
	public delegate void StateChangeDelegate<K>(IState<K> current, IState<K> next);
	
	public interface IStateMachine { }

	public interface IStateMachine<K> : IStateMachine
	{ 
		IState ActiveState { get; }
		event StateChangeDelegate<K> OnStateChange;

		Task SwitchState(IStateData<K> data);
		Task SwitchState(K key);
	}

	public class StateMachine<K> : IStateMachine<K>
	{
		public IState<K> ActiveState { get; protected set; }
		IState IStateMachine<K>.ActiveState => ActiveState;
		protected Dictionary<K, IState<K>> States;
		public event StateChangeDelegate<K> OnStateChange;

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

		private async Task SwitchState(IState<K> state, IStateData<K> data)
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
	}
}
