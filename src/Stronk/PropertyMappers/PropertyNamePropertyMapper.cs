namespace Stronk.PropertyMappers
{
	public class PropertyNamePropertyMapper : IPropertyMapper
	{
		public string Select(PropertyMapperArgs args) => args.GetValue(args.Property.Name);
	}
}
