namespace Utilities.Extensions
{
	public static class EnumerationEx
	{
		private static Random rand = new(Guid.NewGuid().GetHashCode());

		public static (T?, V?) RandomElement<T, V>(this Dictionary<T, V> dict)
			where T : notnull
		{
			if (dict == null || dict.Count == 0) return (default, default);

			KeyValuePair<T, V> keypair = dict.ElementAt(rand.Next(0, dict.Count));
			return (keypair.Key, keypair.Value);
		}

		public static T? RandomElement<T>(this List<T> list)
			=> list == null || list.Count == 0 ? default : list[rand.Next(0, list.Count)];

		public static T? RandomElement<T>(this IEnumerable<T> list)
			=> list == null || !list.Any() ? default : list.ElementAt(rand.Next(0, list.Count()));

		public static T? RandomElement<T>(this T[] list) 
			=> Random(list);

		public static T? Random<T>(params T[] list)
			=> list == null || list.Length == 0 ? default : list[rand.Next(0, list.Length)];

		public static T? RandomElement<T>(Random? random = null) where T : Enum
		{
			random ??= rand;

			T[]? list = (T[])Enum.GetValues(typeof(T));
			if (list is null || list.Length == 0) return default;

			int index = random.Next(0, list.Length);
			return list[index];
		}

	}
}
