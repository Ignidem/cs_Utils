namespace Utilities.Conversions
{
	public interface IConvertible<out T>
	{
		T Convert();
	}
}
