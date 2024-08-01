using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Events
{
	public interface IEventContainer<T> 
		where T : Delegate
	{
		public void Set(T dlgt);
	}
}
