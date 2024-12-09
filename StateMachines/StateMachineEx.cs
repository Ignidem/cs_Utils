using System.Threading.Tasks;

namespace Utils.StateMachines
{
	public static class StateMachineEx
	{
		public static bool IsSwitchingTo<K>(this IStateMachine<K> stateMachine, K key)
		{
			return stateMachine.IsTransitioning && stateMachine.NextState.IsKey(key);
		}

		public static bool IsKey<K>(this IState<K> state, K key) => state.Key.Equals(key);

		public static async Task AwaitSwitch(this IStateMachine machine)
		{
			while (machine.IsTransitioning)
				await Task.Yield();
		}

		public static bool TryActiveStateAs<T>(this IStateMachine stateMachine, out T state)
			where T : IState
		{
			if (stateMachine.ActiveState is T tState)
			{
				state = tState;
				return true;
			}
			state = default;
			return false;
		}
	}
}
