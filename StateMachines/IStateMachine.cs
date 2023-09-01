using System;
using System.Threading.Tasks;

namespace Utils.StateMachines
{
	public delegate void StateChangeDelegate<K>(IState<K> current, IState<K> next);
	public delegate void ExceptionHandlerDelegate(Exception exception);
	
	public interface IStateMachine 
	{
		IState ActiveState { get; }
		event ExceptionHandlerDelegate OnException;
		Task ExitActiveState();
	}

	public interface IStateMachine<K> : IStateMachine
	{
		new IState<K> ActiveState { get; }
		event StateChangeDelegate<K> OnStateChange;
		Task SwitchState(IStateData<K> data);
		Task SwitchState(K key);
	}
	
	public static class StateMachineEx
	{
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
