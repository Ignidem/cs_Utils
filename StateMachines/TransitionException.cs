using System;

namespace Utils.StateMachines
{
	public class TransitionException : Exception
	{
		public readonly IState activeState;
		public readonly IState targetState;

		public TransitionException(IState activeState, IState targetState, Exception inner)
			: base($"An error occured while transition from {activeState?.ToString() ?? "null"} to {targetState?.ToString() ?? "null"}", inner)
		{
			this.activeState = activeState;
			this.targetState = targetState;
		}
	}
}