namespace Utils.EventSystem
{
	public interface IFuncEventHandler<TKey>
	{
		IFuncContainer<TReturn, TArgument> GetFuncContainer<TReturn, TArgument>(TKey key);
		TReturn Invoke<TReturn, TArgument>(TKey key, TArgument arg)
		{
			IFuncContainer<TReturn, TArgument> container = GetFuncContainer<TReturn, TArgument>(key);
			return container == null ? default : container.InvokeEvent(arg);
		}
		void Add<TReturn, TArgument>(TKey key, IFuncContainer<TReturn, TArgument>.EventDelegate func)
		{
			IFuncContainer<TReturn, TArgument> container = GetFuncContainer<TReturn, TArgument>(key);
			container?.Add(func);
		}
		void Remove<TReturn, TArgument>(TKey key, IFuncContainer<TReturn, TArgument>.EventDelegate func)
		{
			IFuncContainer<TReturn, TArgument> container = GetFuncContainer<TReturn, TArgument>(key);
			container?.Remove(func);
		}
	}
}
