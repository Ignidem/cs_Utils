namespace Utils.EventSystem
{
	public class ActionContainer : IActionContainer
	{
		private event IActionContainer.EventDelegate Event;

		public void CleanInstance(object target)
		{
			Event = Event.RemoveTargetInvocation(target);
		}
		public void InvokeEvent()
		{
			if (Event == null) return;

			Event.Invoke();
		}
		public void Add(IActionContainer.EventDelegate func) => Event += func;
		public void Remove(IActionContainer.EventDelegate func) => Event -= func;
		public void RemoveAll() => Event = null;
	}
    public class ActionContainer<TArgument> : IActionContainer<TArgument>
	{
		public int Count => Event?.GetInvocationList().Length ?? 0;
		private event IActionContainer<TArgument>.EventDelegate Event;

        public void CleanInstance(object target)
        {
			Event = Event.RemoveTargetInvocation(target);
        }
        public void InvokeEvent(TArgument args)
        {
            if (Event == null) return;

            Event.Invoke(args);
        }
        public void Add(IActionContainer<TArgument>.EventDelegate func) => Event += func;
        public void Remove(IActionContainer<TArgument>.EventDelegate func) => Event -= func;
		public void RemoveAll() => Event = null;
	}
}
