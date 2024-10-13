namespace Utils.EventSystem
{
	public interface IActionEventHandler<TKey>
	{
		IActionContainer GetActionContainer(TKey key, bool create);

		void Invoke(TKey key)
		{
			IActionContainer container = GetActionContainer(key, false);
			container?.InvokeEvent();
		}
		void Add(TKey key, IActionContainer.EventDelegate func)
		{
			IActionContainer container = GetActionContainer(key, true);
			container.Add(func);
		}
		void Remove(TKey key, IActionContainer.EventDelegate func)
		{
			IActionContainer container = GetActionContainer(key, false);
			container?.Remove(func);
		}
	}

	public interface IArgActionEventHandler<TKey>
	{
		IActionContainer<T> GetActionContainer<T>(TKey key, bool create);
		void Invoke<T>(TKey key, T arg)
		{
			IActionContainer<T> container = GetActionContainer<T>(key, false);
			container?.InvokeEvent(arg);
		}
		void Add<T>(TKey key, IActionContainer<T>.EventDelegate func)
		{
			IActionContainer<T> container = GetActionContainer<T>(key, true);
			container.Add(func);
		}
		void Remove<T>(TKey key, IActionContainer<T>.EventDelegate func)
		{
			IActionContainer<T> container = GetActionContainer<T>(key, false);
			container?.Remove(func);
		}
	}
}
