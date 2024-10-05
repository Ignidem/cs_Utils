using System;

namespace Utils.EventSystem
{
	public interface IEventContainer : IDisposable
	{
		void CleanInstance(object target);
		void RemoveAll();
		void IDisposable.Dispose() => RemoveAll();
	}
}
