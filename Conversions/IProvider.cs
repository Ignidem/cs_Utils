namespace Utilities.Conversions
{
	public interface IProvider<out T>
	{
		T GetValue();
	}
}