namespace Stronk.ValueSelection
{
	public class PropertyNameValueSelector : IValueSelector
	{
		public string Select(ValueSelectorArgs args)
		{
			var propertyName = args.Property.Name;

			return args.AppSettings[propertyName] ?? args.ConnectionStrings[propertyName]?.ConnectionString;
		}
	}
}
