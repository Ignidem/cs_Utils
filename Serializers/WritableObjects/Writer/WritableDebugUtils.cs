using System;
using System.Collections;
using System.Text;

namespace Utils.Serializers.WritableObjects
{
	public interface IWritableDebug
	{
		StringBuilder DebugContent { get; }
		int Indent { get; set; }
	}

	public static class WritableDebugUtils
	{
		public static string GetIndent(this IWritableDebug debug)
		{
			return new string('\t', debug.Indent);
		}

		public static void StartValue(this IWritableDebug debug, Type type)
		{
			debug.DebugContent.Append(debug.GetIndent());
			if (type.IsValueType || type == typeof(string))
				return;

			debug.Indent++;
			if (type.IsArray || typeof(IList).IsAssignableFrom(type))
				debug.DebugContent.Append('[');
			else
				debug.DebugContent.AppendFormat("{0}: {{", type.Name);

			debug.DebugContent.Append('\n');
		}

		public static void Value(this IWritableDebug debug, Type type, object value)
		{
			if (value == null || type.IsValueType || type == typeof(string))
			{
				debug.DebugContent.AppendFormat("{0}: {1}\n", type, value);
			}
		}

		public static void EndValue(this IWritableDebug debug, Type type)
		{
			if (type.IsValueType || type == typeof(string))
				return;

			debug.Indent--;

			if (type.IsArray || typeof(IList).IsAssignableFrom(type))
				debug.DebugContent.Append(debug.GetIndent()).Append("]\n");
			else
				debug.DebugContent.AppendFormat(debug.GetIndent()).Append("}\n");
		}
	}
}