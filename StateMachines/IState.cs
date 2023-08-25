using System.Threading.Tasks;

namespace Utils.StateMachines
{

	public interface IState { }

	public interface IState<K> : IState
	{
		K Key { get; }
		bool IsActive { get; }

		Task Preload(IStateData<K> data);
		Task Enter(IStateMachine<K> stateMachine);
		Task Exit();
		Task Cleanup();
	}

	public abstract class State<K> : IState<K>
	{
		public abstract K Key { get; }
		public bool IsActive { get; private set; }

		private IStateMachine<K> _activeStateMachine;

		public abstract Task Preload(IStateData<K> data);
		public virtual Task Enter(IStateMachine<K> stateMachine) 
		{
			IsActive = true;

			if (_activeStateMachine != stateMachine && _activeStateMachine != null)
			{
				throw new System.Exception(string.Format("State {0}<{1}> is already active in another state machine {2}",
					GetType().Name, Key, _activeStateMachine.GetType().Name));
			}

			_activeStateMachine = stateMachine;

			return Task.CompletedTask;
		}

		public virtual Task Exit()
		{
			IsActive = false;
			_activeStateMachine = null;

			return Task.CompletedTask;
		}

		public abstract Task Cleanup();

		protected Task SwitchState(IStateData<K> data)
		{
			if (!IsActive) return Task.CompletedTask;

			return _activeStateMachine.SwitchState(data);
		}

		protected Task SwitchState(K key)
		{
			if (!IsActive) return Task.CompletedTask;

			return _activeStateMachine.SwitchState(key);
		}
	}
}
