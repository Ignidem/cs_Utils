using System;

namespace Utils.EventSystem
{
	public static class EventContainerEx
	{
		public static T RemoveTargetInvocation<T>(this T delg, object target) where T : Delegate
		{
			if (delg == null) return null;

			Delegate[] evnts = delg.GetInvocationList();
			for (int i = 0; i < evnts.Length; i++)
			{
				Delegate action = evnts[i];
				if (action.Target == target)
					delg = (T)Delegate.RemoveAll(delg, action);
			}

			return delg;
		}
	}
}
