using System.Collections.Generic;
using Stronk.ConfigurationSources;
using Stronk.PropertyWriters;
using Stronk.ValueConverters;

namespace Stronk
{
	public class PropertyConversionUnit
	{
		public PropertyDescriptor Property { get; set; }
		public IValueConverter[] Converters { get; set; }
		public string Value { get; set; }
		public IEnumerable<IConfigurationSource> Sources { get; set; }
	}
}
