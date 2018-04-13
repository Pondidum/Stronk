namespace Stronk.PropertyMappers
{
	public class PropertyNamePropertyMapper : IPropertyMapper
	{
		public string ValueFor(PropertyMapperArgs args) => args.GetValue(args.Property.Name);
	}
}
