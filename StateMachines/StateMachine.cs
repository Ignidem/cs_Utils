using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Utils.Asyncronous;
using Utils.Logger;

namespace Utils.StateMachines
{
	public class StateMachine<K> : IStateMachine<K>, IDisposable
	{
		public IState<K> ActiveState => lastTransition?.ActiveState;
		public IState<K> NextState => lastTransition?.state;

		public bool IsTransitioning => transitionTask != null && !transitionTask.IsCompleted;

		public event StateChangeDelegate<K> OnStateChange;
		public event TransitionDelegate OnTransition;
		public event ExceptionHandlerDelegate OnException;

		protected readonly Dictionary<K, IState<K>> States;
		protected TransitionInfo<K> lastTransition;
		protected Task transitionTask;

		public StateMachine(IEnumerable<IState<K>> states)
		{
			States = states.ToDictionary(state => state.Key, state => state);
		}
		public virtual void Dispose()
		{
			foreach (IState<K> state in States.Values)
			{
				if (state is IDisposable disp)
					disp.Dispose();
			}
		}

		public bool ContainsState(K key)
		{
			return States.ContainsKey(key);
		}
		public void AddOrReplaceState(IState<K> state)
		{
			States[state.Key] = state;
		}

		public virtual async Task SwitchState(IStateData<K> data)
		{
			K key = data.Key;
			if (States.TryGetValue(key, out IState<K> state))
				await SwitchState(state, data);
		}
		public virtual Task SwitchState(IState<K> state)
		{
			return SwitchState(state, null);
		}
		public virtual async Task SwitchState(K key)
		{
			if (States.TryGetValue(key, out IState<K> state))
				await SwitchState(state, null);
		}

		public virtual async Task ExitActiveState()
		{
			try
			{
				IState<K> state = ActiveState;
				if (state == null) return;

				await state?.Exit().TaskOrCompleted();
				await state?.Cleanup().TaskOrCompleted();
			}
			catch (Exception e)
			{
				OnException(e);
			}
		}

		protected void StateChanged(IState<K> state) => OnStateChange?.Invoke(ActiveState, state);
		protected void ExceptionCaught(Exception e) => OnException?.Invoke(e);
		protected void TransitionChanged(TransitionType type) => OnTransition?.Invoke(type);

		protected bool CheckPendingTransition()
		{
			if (!IsTransitioning)
				return false;

			Exception exception = lastTransition.GetTransitionException();
			exception.LogException();
			OnException.Invoke(exception);
			return true;
		}
		protected virtual async Task SwitchState(IState<K> state, IStateData<K> data)
		{
			if (CheckPendingTransition())
				return;

			if (state == null)
			{
				await ExitActiveState();
				return;
			}

			try
			{
				IState<K> exitingState = ActiveState;
				if (state == exitingState)
				{
					await exitingState.Reload(data);
					return;
				}

				lastTransition = new TransitionInfo<K>(exitingState, state, data);
				await (transitionTask = HandleTransition(state, data));
				OnStateChange?.Invoke(exitingState, state);
			}
			catch (Exception e)
			{
				OnException?.Invoke(e);
			}
		}
		public TaskAwaiter GetAwaiter() => (transitionTask ?? Task.CompletedTask).GetAwaiter();
		protected virtual async Task HandleTransition(IState<K> enteringState, IStateData<K> data)
		{
			var exitingState = ActiveState;
			OnTransition?.Invoke(TransitionType.Preload);
			await enteringState.Preload(data);

			if (exitingState != null)
			{
				OnTransition?.Invoke(TransitionType.Exit);
				await exitingState.Exit();
			}

			OnTransition?.Invoke(TransitionType.Enter);
			await enteringState.Enter(this);
			lastTransition.OnEnter();

			if (exitingState != null)
			{
				OnTransition?.Invoke(TransitionType.Cleanup);
				await exitingState?.Cleanup();
			}
		}
	}
}
