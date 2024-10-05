namespace Utils.EventSystem
{
	public interface IActionContainer : IEventContainer
	{
		public delegate void EventDelegate();
		void InvokeEvent();
		void Add(EventDelegate func);
		void Remove(EventDelegate func);
	}
	public interface IActionContainer<TArgument> : IEventContainer
	{
		public delegate void EventDelegate(TArgument arg);
		void Add(EventDelegate func);
		void Remove(EventDelegate func);
		void InvokeEvent(TArgument args);
	}
}
