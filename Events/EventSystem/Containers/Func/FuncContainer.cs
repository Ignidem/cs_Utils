namespace Utils.EventSystem
{
	public class FuncContainer<TReturn, TArgument> : IFuncContainer<TReturn, TArgument>
	{
		public event IFuncContainer<TReturn, TArgument>.EventDelegate Event;

		public void CleanInstance(object target)
		{
			Event = Event.RemoveTargetInvocation(target);
		}
		public TReturn InvokeEvent(TArgument arg)
		{
			if (Event == null) return default;

			return Event.Invoke(arg);
		}
		public void Add(IFuncContainer<TReturn, TArgument>.EventDelegate func) => Event += func;
		public void Remove(IFuncContainer<TReturn, TArgument>.EventDelegate func) => Event -= func;
		public void RemoveAll() => Event = null;
	}
}
