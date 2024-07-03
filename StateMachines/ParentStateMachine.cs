using System.Collections.Generic;
using System.Threading.Tasks;

namespace Utils.StateMachines
{
	public class ParentStateMachine<T> : StateMachine<T>
	{
		private readonly IStateMachine<T> childStateMachine;

		public ParentStateMachine(IEnumerable<IState<T>> states, IStateMachine<T> subStateMachine) 
			: base (states)
		{
			this.childStateMachine = subStateMachine;
		}

		public override Task SwitchState(IStateData<T> data)
		{
			T key = data.Key;
			return States.TryGetValue(key, out IState<T> state) 
				? SwitchState(state, data) 
				: childStateMachine.SwitchState(data);
		}

		public override Task SwitchState(T key)
		{
			return States.TryGetValue(key, out IState<T> state) 
				? SwitchState(state, null) 
				: childStateMachine.SwitchState(key);
		}
	}
}
