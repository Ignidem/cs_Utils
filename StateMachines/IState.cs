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
			await Exit();
			await Cleanup();
			await Preload(data);
			await Enter(machine);
		}

		public Task Preload(IStateData<K> data) => OnPreload(data);

		/// <summary>
		/// First transition method. Invoked before exiting the next state.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Third transition method. Invoked after exiting previous state.
		/// </summary>
		/// <returns></returns>
		protected virtual Task OnEnter() => Task.CompletedTask;

		public Task Exit()
		{
			IsActive = false;
			_activeStateMachine = null;

			return OnExit();
		}

		/// <summary>
		/// Second transition method. Invoked after preloading the next state.
		/// </summary>
		/// <returns></returns>
		protected virtual Task OnExit() => Task.CompletedTask;

		public Task Cleanup() => OnCleanup();

		/// <summary>
		/// Fourth and final transition method. Invoked after entering the next state.
		/// </summary>
		/// <returns></returns>
		protected virtual Task OnCleanup() => Task.CompletedTask;

		protected async Task CloseState()
		{
			if (_activeStateMachine?.ActiveState == this)
			{
				await _activeStateMachine.ExitActiveState();
			}
			else
			{
				await Exit();
				await Cleanup();
			}
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
