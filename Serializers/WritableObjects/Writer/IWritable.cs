namespace Utils.Serializers.WritableObjects
{
	public interface IWritable
	{
		void Write(IWriter writer);
	}

	public interface IWritable<TWriter> : IWritable
		where TWriter : IWriter
	{
		void IWritable.Write(IWriter writer)
		{
			if (writer is not TWriter twriter)
				throw new System.NotImplementedException(GetType().Name);

			Write(twriter);
		}

		void Write(TWriter writer)
		{
			throw new System.NotImplementedException(GetType().Name);
		}
	}
}
