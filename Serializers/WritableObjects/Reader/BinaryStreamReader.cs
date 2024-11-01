using System.IO;

namespace Utils.Serializers.WritableObjects.Reader
{
	public class BinaryStreamReader : IReader
	{
		private readonly Stream stream;
		private readonly BinaryReader reader;

		public BinaryStreamReader(Stream stream) 
		{
			reader = new BinaryReader(stream);
			this.stream = stream;
		}

		public void Dispose()
		{
			reader.Dispose();
			stream.Dispose();
		}

		public T Read<T>()
		{
			throw new System.NotImplementedException();
		}
	}
}
