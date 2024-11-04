namespace Utils.Delegates
{
	public delegate bool HasIdPropertyDelegate<TComponent>(TComponent comp, int id);
	public delegate TValue GetIdPropertyDelegate<TComponent, TValue>(TComponent comp, int id);
	public delegate void SetIdPropertyDelegate<TComponent, TValue>(TComponent comp, int id, TValue value);

	public class IdProperty<TComponent, TValue> : IPropertyDelegate<TComponent>
	{
		public static readonly IdProperty<TComponent, TValue> Empty = 
			new IdProperty<TComponent, TValue>((_, _) => true, (_, _) => default, (_, _, _) => { });

		private readonly HasIdPropertyDelegate<TComponent> hasProperty;
		private readonly GetIdPropertyDelegate<TComponent, TValue> getProperty;
		private readonly SetIdPropertyDelegate<TComponent, TValue> setProperty;

		public IdProperty(
			HasIdPropertyDelegate<TComponent> hasDelegate,
			GetIdPropertyDelegate<TComponent, TValue> getDelegate,
			SetIdPropertyDelegate<TComponent, TValue> setDelegate
			)
		{
			hasProperty = hasDelegate;
			getProperty = getDelegate;
			setProperty = setDelegate;
		}

		public bool Has(TComponent component, int id)
		{
			return hasProperty(component, id);
		}

		public TValue Get(TComponent component, int id)
		{
			if (!Has(component, id))
				throw GetException(component, id);

			return getProperty(component, id);
		}

		public bool TryGet(TComponent component, int id, out TValue value)
		{
			if (!Has(component, id))
			{
				value = default;
				return false;
			}

			value = getProperty(component, id);
			return true;
		}

		public void Set(TComponent component, int id, TValue value)
		{
			if (!Has(component, id))
				throw GetException(component, id);

			setProperty(component, id, value);
		}

		public bool TrySet(TComponent component, int id, TValue value)
		{
			if (!Has(component, id))
				return false;

			setProperty(component, id, value);
			return true;
		}

		private MissingPropertyException GetException(TComponent component, int id)
		{
			return new MissingPropertyException(component, id, typeof(TValue));
		}
	}
}
