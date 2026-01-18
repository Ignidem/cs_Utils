using System;
using System.Collections;
using System.Collections.Generic;

namespace Utils.Collections
{
#nullable enable
	public class SlotList<T> : IReadOnlyList<T>, IEnumerable<KeyValuePair<int, T>>
	{
		public int Count { get; private set; }
		public int Capacity => data.Length;
		public bool IsReadOnly => false;
		
		public T? this[int index]
		{
			get => data[index];
			set
			{
				SetAt(index, value);
				
				T? current = data[index];
				bool hadData = current != null;
				if (hadData)
					Count = hadData ? Count - 1 : Count + 1;
				
				data[index] = value;
			}
		}
		T IReadOnlyList<T>.this[int index] => data[index]!;

		private T?[] data;
		
		#region Constructors
		public SlotList(int capacity = 2)
		{
			data = new T?[capacity];
		}
		#endregion

		private void Resize(int capacity)
		{
			Array.Resize(ref data, capacity);
		}

		private void EnsureCapacity(int capacity)
		{
			int resize = Capacity;
			while (resize < capacity)
			{
				resize *= 2;
			}
			
			if (resize > Capacity)
				Resize(resize);
		}

		public virtual int Add(T? item)
		{
			if (item == null) return -1;
			
			for (int i = 0; i < data.Length; i++)
			{
				if (data[i] != null) continue;
				data[i] = item;
				Count++;
				return i;
			}

			int index = Capacity;
			Resize(data.Length * 2);
			data[index] = item;
			Count++;
			return index;
		}
		public virtual bool SetAt(int index, T? value)
		{
			if (index < 0) return false;
			
			if (value == null)
			{
				T? removed = RemoveAt(index);
				return removed != null;
			}
			
			if (index >= data.Length)
				EnsureCapacity(index + 1);
			
			T? old = data[index];
			data[index] = value;
			if (old == null) 
				Count++;
			return true;
		}
		public virtual T? RemoveAt(int index)
		{
			if (index < 0 || index >= data.Length) return default;
			T? item = data[index];
			data[index] = default;
			if (item != null)
				Count--;
			return item;
		}
		public virtual void Clear()
		{			
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = default;
			}

			Count = 0;
		}

		#region Enumerators
		IEnumerator<KeyValuePair<int, T>> IEnumerable<KeyValuePair<int, T>>.GetEnumerator()
		{			
			for (int i = 0; i < Capacity; i++)
			{
				T? item = data[i];
				if (item != null) 
					yield return new KeyValuePair<int, T>(i, item);
			}
		}
		public IEnumerator<T> GetEnumerator()
		{
			for (int i = 0; i < Capacity; i++)
			{
				T? item = data[i];
				if (item != null) yield return item;
			}
		}
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		#endregion
	}
#nullable restore
}