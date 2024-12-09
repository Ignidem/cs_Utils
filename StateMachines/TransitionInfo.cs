using System;

namespace Utils.StateMachines
{
	public class TransitionInfo<K>
	{
		public IState<K> ActiveState => hasEntered ? state : previousState;

		public readonly IState<K> previousState;
		public readonly IState<K> state;
		public readonly IStateData<K> data;

		private bool hasEntered;

		public TransitionInfo(IState<K> previousState, IState<K> state, IStateData<K> data)
		{
			this.previousState = previousState;
			this.state = state;
			this.data = data;
		}

		public void OnEnter()
		{
			hasEntered = true;
		}

		public Exception GetTransitionException()
		{
			return new Exception("StateMachine is already switching states!\n" +
					$"Active Switch: [{previousState}] To [{state}] with data [{data}]");
		}
	}
}
