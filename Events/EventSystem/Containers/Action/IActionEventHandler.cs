namespace Utils.EventSystem
{
	public interface IActionEventHandler<TKey>
	{
		IActionContainer GetActionContainer(TKey key);

		void Invoke(TKey key)
		{
			IActionContainer container = GetActionContainer(key);
			container?.InvokeEvent();
		}
		void Add(TKey key, IActionContainer.EventDelegate func)
		{
			IActionContainer container = GetActionContainer(key);
			container?.Add(func);
		}
		void Remove(TKey key, IActionContainer.EventDelegate func)
		{
			IActionContainer container = GetActionContainer(key);
			container?.Remove(func);
		}
	}

	public interface IArgActionEventHandler<TKey>
	{
		IActionContainer<T> GetActionContainer<T>(TKey key);
		void Invoke<T>(TKey key, T arg)
		{
			IActionContainer<T> container = GetActionContainer<T>(key);
			container?.InvokeEvent(arg);
		}
		void Add<T>(TKey key, IActionContainer<T>.EventDelegate func)
		{
			IActionContainer<T> container = GetActionContainer<T>(key);
			container?.Add(func);
		}
		void Remove<T>(TKey key, IActionContainer<T>.EventDelegate func)
		{
			IActionContainer<T> container = GetActionContainer<T>(key);
			container?.Remove(func);
		}
	}
}
