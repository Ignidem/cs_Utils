using System.Threading.Tasks;

namespace Assets.External.Utils.StateMachines
{

	public interface IState { }

	public interface IState<K> : IState
	{
		K Key { get; }

		Task Enter(IStateData<K> data);
		Task Exit();
	}

	public abstract class State<K> : IState<K>
	{
		public abstract K Key { get; }

		public virtual Task Enter(IStateData<K> data) => Task.CompletedTask;
		public virtual Task Exit() => Task.CompletedTask;
	}
}
