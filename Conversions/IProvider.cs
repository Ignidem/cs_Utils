namespace Utilities.Conversions
{
	public interface IProvider<out T>
	{
		T GetValue();
	}

	public interface IProvider<out T, in P>
	{
		T GetValue(P parameter);
	}
}