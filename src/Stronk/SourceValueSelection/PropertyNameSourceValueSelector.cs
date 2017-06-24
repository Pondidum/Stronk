namespace Stronk.SourceValueSelection
{
	public class PropertyNameSourceValueSelector : ISourceValueSelector
	{
		public string Select(ValueSelectorArgs args)
		{
			var propertyName = args.Property.Name;

			string value = null;
			args.AppSettings.TryGetValue(propertyName, out value);

			return value ?? args.ConnectionStrings[propertyName]?.ConnectionString;
		}
	}
}
