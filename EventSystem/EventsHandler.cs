using System.Collections.Generic;

namespace Utils.EventSystem
{
	public class EventHandler<TKey> : IEventHandler<TKey>
	{
        private readonly Dictionary<TKey, IEventContainer> eventsContainers
			= new Dictionary<TKey, IEventContainer>();

		#region Action
		public void Invoke(TKey key)
		{
			EventContainer container = Get<EventContainer>(key);
			container?.InvokeEvent();
		}
		public void Add(TKey key, EventContainer.EventDelegate func)
		{
			EventContainer container = Get<EventContainer>(key);
			container?.Add(func);
		}
		public void Remove(TKey key, EventContainer.EventDelegate func)
		{
			EventContainer container = Get<EventContainer>(key);
			container?.Remove(func);
		}
		#endregion

		#region Action In
		public void Invoke<T>(TKey key, T arg)
		{
			EventContainer<T> container = Get<EventContainer<T>>(key);
			container?.InvokeEvent(arg);
		}
		public void Add<T>(TKey key, EventContainer<T>.EventDelegate func)
		{
			EventContainer<T> container = Get<EventContainer<T>>(key);
			container?.Add(func);
		}
		public void Remove<T>(TKey key, EventContainer<T>.EventDelegate func)
		{
			EventContainer<T> container = Get<EventContainer<T>>(key);
			container?.Remove(func);
		}
		#endregion

		#region Func in out
		public TReturn Invoke<TReturn, TArgument>(TKey key, TArgument arg)
        {
            EventContainer<TReturn, TArgument> container = Get<EventContainer<TReturn, TArgument>>(key);
            return container == null ? default : container.InvokeEvent(arg);
        }
		public void Add<TSource, TArgument>(TKey key, EventContainer<TSource, TArgument>.EventDelegate func)
        {
			EventContainer<TSource, TArgument> container = Get<EventContainer<TSource, TArgument>>(key);
			container?.Add(func);
        }

        public void Remove<TSource, TArgument>(TKey key, EventContainer<TSource, TArgument>.EventDelegate func)
        {
            EventContainer<TSource, TArgument> container = Get<EventContainer<TSource, TArgument>>(key);
            container?.Remove(func);
        }
		#endregion

		public void CleanInstace(object target)
        {
            foreach (KeyValuePair<TKey, IEventContainer> keypair in eventsContainers)
            {
                keypair.Value.CleanInstance(target);
            }
        }

        private T Get<T>(TKey key) 
			where T : IEventContainer, new()
		{
            if (!eventsContainers.TryGetValue(key, out IEventContainer container))
            {
                container = new T();
                eventsContainers.Add(key, container);
            }

            if (container is T eventContainer)
                return eventContainer;

			const string messageFormat = "Event Container {0} at Key {1} does not match types {2}";
			string message = string.Format(messageFormat, container.GetType().Name, key, typeof(T).Name);

			throw new System.Exception(message);
        }
    }
}
