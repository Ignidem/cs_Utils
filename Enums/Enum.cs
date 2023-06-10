using Utilities.Extensions;

namespace Utilities.Enums
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

	public static class EnumParser
	{
		private class EnumInfo
		{
			private readonly Dictionary<string, object> parse = new();
			private readonly Dictionary<object, string> toString = new();

			public EnumInfo(Type type)
			{
				Array values = Enum.GetValues(type);

				foreach(object value in values)
				{
					string name = value.ToString()!.Replace('_', ' ');
					toString[value] = name;
					parse[name.ToLower()] = value;
				}
			}

			public bool TryParse(string input, out object? obj)
				=> parse.TryGetValue(input.ToLower(), out obj);

			public bool TryToString(object input, out string? name)
				=> toString.TryGetValue(input, out name);
		}

		private static readonly Dictionary<Type, EnumInfo> enumsInfo = new();

		private static EnumInfo GetOrCreate(Type type)
		{
			if (!enumsInfo.TryGetValue(type, out var info))
				enumsInfo[type] = info = new(type);
			return info;
		}

		public static bool TryParseEnum(this string input, Type type, out object? enumValue)
		{
			if (!type.IsEnum) throw new ArgumentException($"Type {type} is not an enum."); 

			return PrivateTryParseEnum(input, type, out enumValue);
		}

		public static bool TryParseEnum<T>(this string input, out T enumValue)
			where T : Enum
		{
			Type type = typeof(T);

			if (PrivateTryParseEnum(input, type, out object? obj))
			{
				enumValue = (T)obj!;
				return true;
			}

			enumValue = default!;
			return false;
		}

		public static string? ToName<T>(this T enumValue) where T : Enum
		{
			Type type = typeof(T);

			return PrivateToName(enumValue, type);
		}

		private static bool PrivateTryParseEnum(string input, Type type, out object? enumValue)
		{
			EnumInfo info = GetOrCreate(type);

			return info.TryParse(input, out enumValue);
		}

		private static string? PrivateToName(object enumValue, Type type)
		{
			EnumInfo info = GetOrCreate(type);

			info.TryToString(enumValue, out string? name);

			return name;
		}
	}
}
