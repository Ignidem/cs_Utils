namespace Utils.EventSystem
{
	public interface IFuncContainer<TReturn, TArgument> : IEventContainer
	{
		public delegate TReturn EventDelegate(TArgument arg);
		void Add(EventDelegate func);
		void Remove(EventDelegate func);
		TReturn InvokeEvent(TArgument args);
	}
}
