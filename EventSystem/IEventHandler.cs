namespace Utils.EventSystem
{
	public interface IEventHandler { }

	public interface IEventHandler<TKey>
	{
		void Invoke(TKey key);
		void Add(TKey key, EventContainer.EventDelegate func);
		void Remove(TKey key, EventContainer.EventDelegate func);

		void Invoke<T>(TKey key, T arg);
		void Add<T>(TKey key, EventContainer<T>.EventDelegate func);
		void Remove<T>(TKey key, EventContainer<T>.EventDelegate func);

		TReturn Invoke<TReturn, TArgument>(TKey key, TArgument arg);
		void Add<TSource, TArgument>(TKey key, EventContainer<TSource, TArgument>.EventDelegate func);
		void Remove<TSource, TArgument>(TKey key, EventContainer<TSource, TArgument>.EventDelegate func);

		void CleanInstace(object target);
	}
}
