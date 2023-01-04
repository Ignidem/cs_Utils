using Utilities.Extensions;

namespace IBS_Web.External.CS.Utils.Enums
{
	public class Enum<T> where T : Enum
	{
		public static T? Random(Random random)
			=> EnumerationEx.RandomElement<T>(random);

		public static implicit operator T(Enum<T> e) => e.Value;

		public static implicit operator Enum<T>(T e) => new() { Value = e };

		public static bool TryParse(object o, out T t)
		{
			if (o is T e) return (t = e) != null;

			if (o is int)
			{
				try
				{
					return (t = (T)o) != null;
				}
				catch (Exception) { }
			}

			if (o is string s) return TryParse(s, out t);

			t = default!;
			return false;
		}

		public static bool TryParse(string value, out T t)
		{
			return (t = default!) != null;
		}

		public T Value { get; set; }
	}
}
