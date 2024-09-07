using System;

namespace Utils.EventSystem
{
	public interface IEventContainer : IDisposable
	{
		void CleanInstance(object target);
		void RemoveAll();
		void IDisposable.Dispose() => RemoveAll();
	}

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

	public interface IFuncContainer<TReturn, TArgument> : IEventContainer
	{
		public delegate TReturn EventDelegate(TArgument arg);
		void Add(EventDelegate func);
		void Remove(EventDelegate func);
		TReturn InvokeEvent(TArgument args);
	}
}
