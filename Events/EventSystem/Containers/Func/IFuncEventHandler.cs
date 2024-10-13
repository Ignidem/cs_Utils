namespace Utils.EventSystem
{
	public interface IFuncEventHandler<TKey>
	{
		IFuncContainer<TReturn, TArgument> GetFuncContainer<TReturn, TArgument>(TKey key, bool create);
		TReturn Invoke<TReturn, TArgument>(TKey key, TArgument arg)
		{
			IFuncContainer<TReturn, TArgument> container = GetFuncContainer<TReturn, TArgument>(key, false);
			return container == null ? default : container.InvokeEvent(arg);
		}
		void Add<TReturn, TArgument>(TKey key, IFuncContainer<TReturn, TArgument>.EventDelegate func)
		{
			IFuncContainer<TReturn, TArgument> container = GetFuncContainer<TReturn, TArgument>(key, true);
			container.Add(func);
		}
		void Remove<TReturn, TArgument>(TKey key, IFuncContainer<TReturn, TArgument>.EventDelegate func)
		{
			IFuncContainer<TReturn, TArgument> container = GetFuncContainer<TReturn, TArgument>(key, false);
			container?.Remove(func);
		}
	}
}
