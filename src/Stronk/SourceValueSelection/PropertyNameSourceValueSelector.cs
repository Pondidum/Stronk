using System.Configuration;

namespace Stronk.SourceValueSelection
{
	public class PropertyNameSourceValueSelector : ISourceValueSelector
	{
		public string Select(ValueSelectorArgs args)
		{
			var propertyName = args.Property.Name;

			string value = null;
			args.AppSettings.TryGetValue(propertyName, out value);

			if (value != null)
				return value;

			ConnectionStringSettings connection = null;
			args.ConnectionStrings.TryGetValue(propertyName, out connection);

			return connection?.ConnectionString;
		}
	}
}
