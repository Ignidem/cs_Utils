using System;
using System.Threading.Tasks;

namespace Utils.StateMachines
{
	public delegate void StateChangeDelegate<K>(IState<K> current, IState<K> next);
	public delegate void ExceptionHandlerDelegate(Exception exception);
	
	public interface IStateMachine 
	{
		IState ActiveState { get; }
		bool IsSwitching { get; }
		event ExceptionHandlerDelegate OnException;
		Task ExitActiveState();
	}

	public interface IStateMachine<K> : IStateMachine
	{
		public readonly struct SwitchInfo
		{
			public bool IsSwitching => newState != null;

			public readonly IState<K> oldState;
			public readonly IState<K> newState;
			public readonly IStateData<K> newStateData;

			public SwitchInfo(IState<K> oldState, IState<K> newState, IStateData<K> newStateData)
			{
				this.oldState = oldState;
				this.newState = newState;
				this.newStateData = newStateData;
			}
		}

		new IState<K> ActiveState { get; }
		IState IStateMachine.ActiveState => ActiveState;
		SwitchInfo ActiveSwitch { get; }
		bool IStateMachine.IsSwitching => ActiveSwitch.IsSwitching;

		/// <summary>
		/// Invoked when the states switch cycle is complete. 
		/// (After completely exiting previous state and fully entering next state.)
		/// </summary>
		event StateChangeDelegate<K> OnStateChange;
		bool ContainsState(K key);
		void AddOrReplaceState(IState<K> state);
		Task SwitchState(IState<K> state);
		Task SwitchState(IStateData<K> data);
		Task SwitchState(K key);
	}
}
