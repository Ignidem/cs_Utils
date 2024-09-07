namespace Utils.EventSystem
{
	public interface IEventHandler { }

	public interface IEventHandler<TKey>
	{
		void Invoke(TKey key);
		void Add(TKey key, IActionContainer.EventDelegate func);
		void Remove(TKey key, IActionContainer.EventDelegate func);

		void Invoke<T>(TKey key, T arg);
		void Add<T>(TKey key, IActionContainer<T>.EventDelegate func);
		void Remove<T>(TKey key, IActionContainer<T>.EventDelegate func);

		TReturn Invoke<TReturn, TArgument>(TKey key, TArgument arg);
		void Add<TSource, TArgument>(TKey key, IFuncContainer<TSource, TArgument>.EventDelegate func);
		void Remove<TSource, TArgument>(TKey key, IFuncContainer<TSource, TArgument>.EventDelegate func);

		void CleanInstace(object target);
	}
}
