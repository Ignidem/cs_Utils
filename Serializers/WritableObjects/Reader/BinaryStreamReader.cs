namespace Utils.Serializers.WritableObjects.Reader
{
	public abstract class BinaryStreamReader<TReader, TWriter> : IReader
		where TReader : IReader
		where TWriter : IWriter
	{
		private static readonly Dictionary<Type, Delegate> readerFunctions = new();

		protected readonly Stream stream;
		protected readonly BinaryReader reader;

		public BinaryStreamReader(Stream stream) 
		{
			reader = new BinaryReader(stream);
			this.stream = stream;
		}

		public virtual void Dispose()
		{
			reader.Dispose();
			stream.Dispose();
			GC.SuppressFinalize(this);
		}

		public virtual T Read<T>()
		{
			if (this.reader.TryRead(out T value))
				return value;

			Func<TReader, T> saveRead = GenericWritable<TReader, TWriter>.GetReader<T>();
			readerFunctions[typeof(T)] = saveRead;
			if (this is not TReader reader)
			{
				throw new Exception("Invalid Reader Type");
			}

			return saveRead(reader);
		}
	}
}
