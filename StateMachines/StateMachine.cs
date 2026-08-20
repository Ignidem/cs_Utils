using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Utils.Logger;
using Utils.Results;

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

		public virtual async Task<Result> SwitchState(IStateData<K> data)
		{
			K key = data.Key;
			if (States.TryGetValue(key, out IState<K> state))
				return await SwitchState(state, data);
			
			return "State not found";
		}
		public virtual Task<Result> SwitchState(IState<K> state)
		{
			return SwitchState(state, null);
		}
		public virtual async Task<Result> SwitchState(K key)
		{
			if (States.TryGetValue(key, out IState<K> state))
				return await SwitchState(state, null);
			
			return "State not found";
		}

		public virtual async Task ExitActiveState()
		{
			IState<K> state = ActiveState;
			
			try
			{
				if (state == null)
				{
					lastTransition = null;
					transitionTask = null;
					return;
				}

				static async Task Exit(IState<K> state)
				{
					await state.Exit();
					await state.Cleanup();
				}

				lastTransition = new TransitionInfo<K>(state, null, null);
				await (transitionTask = Exit(state));
			}
			catch (Exception e)
			{
				OnException?.Invoke(e);
				throw new TransitionException(state, null, e);
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
			OnException?.Invoke(exception);
			return true;
		}

		protected virtual async Task<Result> SwitchState(IState<K> state, IStateData<K> data)
		{
			if (CheckPendingTransition())
				return "Active Transition";

			if (state == null)
			{
				await ExitActiveState();
				return true;
			}

			IState<K> exitingState = ActiveState;
			try
			{
				if (state == exitingState)
				{
					transitionTask = exitingState.Reload(data);
					await transitionTask;
					return true;
				}

				lastTransition = new TransitionInfo<K>(exitingState, state, data);
				transitionTask = HandleTransition(state, data);
				await transitionTask;
				OnStateChange?.Invoke(exitingState, state);
				return true;
			}
			catch (Exception e)
			{
				OnException?.Invoke(e);
				return new TransitionException(exitingState, state, e);
			}
		}
		public TaskAwaiter GetAwaiter() => (transitionTask ?? Task.CompletedTask).GetAwaiter();
		protected virtual async Task HandleTransition(IState<K> enteringState, IStateData<K> data)
		{
			IState<K> exitingState = ActiveState;
			OnTransition?.Invoke(TransitionType.Preload);
			await enteringState.Preload(data);

			if (exitingState != null)
			{
				OnTransition?.Invoke(TransitionType.Exit);
				Task exitTask = exitingState.Exit();
				await exitTask;
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
