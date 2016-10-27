using System.Collections.Specialized;
using System.Configuration;
using Stronk.PropertySelection;

namespace Stronk.ValueSelection
{
	public class ValueSelectorArgs
	{
		public NameValueCollection AppSettings { get; }
		public ConnectionStringSettingsCollection ConnectionStrings { get; }
		public PropertyDescriptor Property { get; private set; }

		internal ValueSelectorArgs(NameValueCollection appSettings, ConnectionStringSettingsCollection connectionStrings)
		{
			AppSettings = appSettings;
			ConnectionStrings = connectionStrings;
		}

		public ValueSelectorArgs With(PropertyDescriptor property)
		{
			Property = property;
			return this;
		}
	}
}
