using System.Text;

namespace Utilities.Extensions
{
	public static class StringEx
	{
		public static string ToReadable(this string text)
		{
			StringBuilder sb = new(text);

			for (int i = 0; i < sb.Length; i++)
			{
				char c = sb[i];
				switch (c)
				{
					case '_':
						sb[i] = ' ';
						continue;
				}

				if (i == 0)
				{
					sb[i] = char.ToUpper(c);
				}
				else
				{
					if (char.IsUpper(c) && char.IsLetter(sb[i-1]))
					{
						sb.Insert(i, ' ');
						continue;
					}
				}
			}

			return sb.ToString();
		}
	}
}
