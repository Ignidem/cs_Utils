using System.Reflection;
using System.Text.RegularExpressions;
using Utilities.Reflection.Members;

namespace Utilities.Reflection
{
	public class ReflectionContext
	{
		private static readonly Regex IndexerRegex = new(@"\[[0-9]+\]$");

		private const BindingFlags bindingFlags =
				BindingFlags.Instance |	BindingFlags.Public |
				BindingFlags.NonPublic | BindingFlags.IgnoreCase;

		private static bool Relativity(string path, ref int pointer, ref ReflectionContext context)
		{
			if (path[pointer] != '.') return false;
			
			pointer++;
			if (path[pointer] != '.') return true;
			
			pointer++;

			if (context.parent == null)
				throw new Exception("Context has no parent.");

			context = context.parent;

			return true;
		}
		
		private static ReflectionContext? ArrayField(string fieldName, ReflectionContext context)
		{
			Match match = IndexerRegex.Match(fieldName);

			if (!match.Success) return null;

			if (!int.TryParse(match.Value[1..^1], out int index)) index = 0;

			fieldName = fieldName[..match.Index];

			Member? member = (Member?)context.type.GetField(fieldName, bindingFlags) 
				?? (Member?)context.type.GetProperty(fieldName, bindingFlags);

			if (member is null) return null;

			object? target = member.Read(context.target);

			if (target is null) return null;

			ReflectionContext parent = new(target, context, member);

			PropertyInfo? indexer = parent.type.GetProperties()
				.FirstOrDefault(p => p.GetIndexParameters().Length > 0);

			if(indexer is null) return null;

			target = indexer.GetValue(target, new object[] { index });

			return target is null ? null : new ReflectionContext(target, parent, indexer);
		}

		private static ReflectionContext? GetField(string fieldName, ReflectionContext context)
		{
			Member? member = (Member?)context.type.GetField(fieldName, bindingFlags)
				?? (Member?)context.type.GetProperty(fieldName, bindingFlags);

			if (member is null) return null;

			object? target = member.Read(context.target);

			return new ReflectionContext(target, context, member);
		}

		public readonly object? target;

		public readonly Type type;

		public readonly ReflectionContext? parent;
		public readonly Member? origin;

		public ReflectionContext(object? target, 
			ReflectionContext? parent = null,
			Member? origin = null)
		{
			this.target = target;
			type = target?.GetType() ?? origin?.ValueType()!;

			this.parent = parent;
			this.origin = origin;
		}

		public ReflectionContext? ReadPath(string path)
		{
			int pointer = 0;
			ReflectionContext context = this;

			while(pointer < path.Length)
			{
				if (Relativity(path, ref pointer, ref context)) continue;

				int end = path.IndexOf('.', pointer);

				if(end == -1) end = path.Length;

				string field = path[pointer..end];

				ReflectionContext? result = 
					ArrayField(field, context) ??
					GetField(field, context);

				if (result is null) return null;

				context = result;

				pointer = end + 1;
			}

			return context;
		}

		public void SetValue(object? value)
		{
			if (parent is null || origin is null) return;

			origin.Write(parent.target, value);

			if (!parent.type.IsStruct()) return;

			parent.SetValue(parent.target);
		}
	}
}
