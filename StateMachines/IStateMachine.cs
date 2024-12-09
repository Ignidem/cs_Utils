using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Utils.StateMachines
{
	public delegate void StateChangeDelegate<K>(IState<K> current, IState<K> next);
	public delegate void TransitionDelegate(TransitionType type);
	public delegate void ExceptionHandlerDelegate(Exception exception);
	
	public interface IStateMachine 
	{
		IState ActiveState { get; }
		bool IsTransitioning { get; }
		event ExceptionHandlerDelegate OnException;
		Task ExitActiveState();
		TaskAwaiter GetAwaiter();
	}

	public partial interface IStateMachine<K> : IStateMachine
	{
		new IState<K> ActiveState { get; }
		IState<K> NextState { get; }
		IState IStateMachine.ActiveState => ActiveState;

		/// <summary>
		/// Invoked when the states switch cycle is complete. 
		/// (After completely exiting previous state and fully entering next state.)
		/// </summary>
		event StateChangeDelegate<K> OnStateChange;
		event TransitionDelegate OnTransition;
		bool ContainsState(K key);
		void AddOrReplaceState(IState<K> state);
		Task SwitchState(IState<K> state);
		Task SwitchState(IStateData<K> data);
		Task SwitchState(K key);
	}
}
