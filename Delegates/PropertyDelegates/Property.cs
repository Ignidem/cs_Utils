namespace Utils.Delegates
{
	public delegate TValue GetPropertyDelegate<TComponent, TValue>(TComponent comp);
	public delegate void SetPropertyDelegate<TComponent, TValue>(TComponent comp, TValue value);

	public class Property<TComponent, TValue> : IPropertyDelegate<TComponent>
	{
		public static readonly Property<TComponent, TValue> Empty =
			new Property<TComponent, TValue>((_) => default, (_, _) => { });

		private readonly GetPropertyDelegate<TComponent, TValue> getProperty;
		private readonly SetPropertyDelegate<TComponent, TValue> setProperty;

		public Property(
			GetPropertyDelegate<TComponent, TValue> getDelegate,
			SetPropertyDelegate<TComponent, TValue> setDelegate
			)
		{
			getProperty = getDelegate;
			setProperty = setDelegate;
		}

		public TValue Get(TComponent component)
		{
			return getProperty(component);
		}
		public void Set(TComponent component, TValue value)
		{
			setProperty(component, value);
		}
	}
}
