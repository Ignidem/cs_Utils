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
		event StateChangeDelegate<K> OnStateChange;
		Task SwitchState(IStateData<K> data);
		Task SwitchState(K key);
	}
	
	public static class StateMachineEx
	{
		public static bool IsSwitchingTo<K>(this IStateMachine<K> stateMachine, K key)
		{
			var info = stateMachine.ActiveSwitch;
			return info.IsSwitching && info.newState.Key.Equals(key);
		}

		public static bool TryActiveStateAs<T>(this IStateMachine stateMachine, out T state)
			where T : IState
		{
			if (stateMachine.ActiveState is T tState)
			{
				state = tState;
				return true;
			}
			state = default;
			return false;
		}
	}
}
