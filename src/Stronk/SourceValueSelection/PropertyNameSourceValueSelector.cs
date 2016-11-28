namespace Stronk.SourceValueSelection
{
	public class PropertyNameSourceValueSelector : ISourceValueSelector
	{
		public string Select(ValueSelectorArgs args)
		{
			var propertyName = args.Property.Name;

			return args.AppSettings[propertyName] ?? args.ConnectionStrings[propertyName]?.ConnectionString;
		}
	}
}
