using System;

namespace Utils.EventSystem
{
	public interface IEventContainer
	{
		void CleanInstance(object target);
	}

    public class EventContainer : IEventContainer
	{
		public delegate void EventDelegate();

		private event EventDelegate Event;

		public void CleanInstance(object target)
		{
			Event = Event.RemoveTargetInvocation(target);
		}

		public void InvokeEvent()
		{
			if (Event == null) return;

			Event.Invoke();
		}

		public void Add(EventDelegate func)
			=> Event += func;

		public void Remove(EventDelegate func)
			=> Event -= func;
	}

    public class EventContainer<TArgument> : IEventContainer
	{
		public delegate void EventDelegate(TArgument args);

		private event EventDelegate Event;

        public void CleanInstance(object target)
        {
			Event = Event.RemoveTargetInvocation(target);
        }

        public void InvokeEvent(TArgument args)
        {
            if (Event == null) return;

            Event.Invoke(args);
        }

        public void Add(EventDelegate func)
            => Event += func;

        public void Remove(EventDelegate func)
            => Event -= func;
    }

	public class EventContainer<TReturn, TArgument> : IEventContainer
	{
		public delegate TReturn EventDelegate(TArgument arg);

		public event EventDelegate Event;

		public void CleanInstance(object target)
		{
			Event = Event.RemoveTargetInvocation(target);
		}

		public TReturn InvokeEvent(TArgument arg)
		{
			if (Event == null) return default;

			return Event.Invoke(arg);
		}

		public void Add(EventDelegate func)
			=> Event += func;

		public void Remove(EventDelegate func)
			=> Event -= func;
	}
}
