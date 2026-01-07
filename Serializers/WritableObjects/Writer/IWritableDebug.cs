using System.Text;
using Utils.Logger;

namespace Utils.Serializers.WritableObjects
{
	public interface IWritableDebug
	{
		StringBuilder DebugContent { get; }
		int Indent { get; set; }
	}

	public class WritableDebug : IWritableDebug
	{
		public StringBuilder DebugContent { get; }
		public int Indent { get; set; }
		
		public WritableDebug(string name)
		{
			DebugContent = new StringBuilder(name + " Data:\n");
		}

		public void Log()
		{
			DebugContent.ToString().LogMessage();
		}
	}
}