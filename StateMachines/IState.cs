using System;
using System.Threading.Tasks;

namespace Utils.StateMachines
{
	public interface IState 
	{
		Task Exit();
		Task Cleanup();
	}

	public interface IState<K> : IState
	{
		K Key { get; }
		bool IsActive { get; }

		Task Reload(IStateData<K> data);
		Task Preload(IStateData<K> data);
		Task Enter(IStateMachine<K> stateMachine);
	}

	public abstract class State<K> : IState<K>
	{
		public abstract K Key { get; }
		public bool IsActive { get; private set; }

		private IStateMachine<K> _activeStateMachine;

		public async Task Reload(IStateData<K> data)
		{
			IStateMachine<K> machine = _activeStateMachine;
			await Cleanup();
			await Exit();
			await Preload(data);
			await Enter(machine);
		}

		public Task Preload(IStateData<K> data) => OnPreload(data);
		protected virtual Task OnPreload(IStateData<K> data) => Task.CompletedTask;

		public Task Enter(IStateMachine<K> stateMachine) 
		{
			IsActive = true;

			if (_activeStateMachine != stateMachine && _activeStateMachine != null)
			{
				throw new Exception(string.Format("State {0}<{1}> is already active in another state machine {2}",
					GetType().Name, Key, _activeStateMachine.GetType().Name));
			}

			_activeStateMachine = stateMachine;

			return OnEnter();
		}
		protected virtual Task OnEnter() => Task.CompletedTask;

		public Task Exit()
		{
			IsActive = false;
			_activeStateMachine = null;

			return OnExit();
		}
		protected virtual Task OnExit() => Task.CompletedTask;

		public Task Cleanup() => OnCleanup();
		protected virtual Task OnCleanup() => Task.CompletedTask;

		protected async Task CloseState()
		{
			await Cleanup();
			await Exit();
		}

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
