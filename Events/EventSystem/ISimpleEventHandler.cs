namespace Utils.EventSystem
{
	public interface ISimpleEventHandler<TKey> : IEventHandler<TKey>,
		IActionEventHandler<TKey>, IArgActionEventHandler<TKey>, IFuncEventHandler<TKey>
	{ }
}
