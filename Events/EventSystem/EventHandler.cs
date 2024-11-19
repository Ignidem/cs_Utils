using System.Collections.Generic;

namespace Utils.EventSystem
{
	public class EventHandler<TKey> : IEventHandler<TKey>,
		IActionEventHandler<TKey>, IArgActionEventHandler<TKey>, IFuncEventHandler<TKey>
	{
        private readonly Dictionary<TKey, IEventContainer> eventsContainers
			= new Dictionary<TKey, IEventContainer>();

		#region Action		
		public virtual IActionContainer GetActionContainer(TKey key, bool create)
		{
			return GetContainer<ActionContainer>(key, create);
		}
		public void Invoke(TKey key)
		{
			IActionContainer container = GetActionContainer(key, false);
			container?.InvokeEvent();
		}
		public void Add(TKey key, IActionContainer.EventDelegate func)
		{
			IActionContainer container = GetActionContainer(key, true);
			container.Add(func);
		}
		public void Remove(TKey key, IActionContainer.EventDelegate func)
		{
			IActionContainer container = GetActionContainer(key, false);
			container?.Remove(func);
		}
		#endregion

		#region Action In
		public virtual IActionContainer<T> GetActionContainer<T>(TKey key, bool create)
		{
			return GetContainer<ActionContainer<T>>(key, create);
		}
		public void Invoke<T>(TKey key, T arg)
		{
			IActionContainer<T> container = GetActionContainer<T>(key, false);
			container?.InvokeEvent(arg);
		}
		public void Add<T>(TKey key, IActionContainer<T>.EventDelegate func)
		{
			IActionContainer<T> container = GetActionContainer<T>(key, true);
			container.Add(func);
		}
		public void Remove<T>(TKey key, IActionContainer<T>.EventDelegate func)
		{
			IActionContainer<T> container = GetActionContainer<T>(key, false);
			container?.Remove(func);
		}
		#endregion

		#region Func in out
		public virtual IFuncContainer<TReturn, TArgument> GetFuncContainer<TReturn, TArgument>(TKey key, bool create)
		{
			return GetContainer<FuncContainer<TReturn, TArgument>>(key, create);
		}
		public TReturn Invoke<TReturn, TArgument>(TKey key, TArgument arg)
        {
            IFuncContainer<TReturn, TArgument> container = GetFuncContainer<TReturn, TArgument>(key, false);
            return container == null ? default : container.InvokeEvent(arg);
        }
		public void Add<TReturn, TArgument>(TKey key, IFuncContainer<TReturn, TArgument>.EventDelegate func)
		{
			IFuncContainer<TReturn, TArgument> container = GetFuncContainer<TReturn, TArgument>(key, true);
			container?.Add(func);
        }
        public void Remove<TReturn, TArgument>(TKey key, IFuncContainer<TReturn, TArgument>.EventDelegate func)
		{
			IFuncContainer<TReturn, TArgument> container = GetFuncContainer<TReturn, TArgument>(key, false);
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

        public T GetContainer<T>(TKey key, bool create) 
			where T : IEventContainer, new()
		{
            if (!eventsContainers.TryGetValue(key, out IEventContainer container))
            {
				if (!create)
				{
					//When invoking or removing, there is no need to create a missing container;
					return default;
				}

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
