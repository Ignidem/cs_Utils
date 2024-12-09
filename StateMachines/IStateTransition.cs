namespace Utils.StateMachines
{
	public interface IStateTransition<K>
	{
		TransitionType State { get; }
		IState<K> ActiveState { get; }
		bool IsTransitioning { get; }
	}
}
