using Stronk.ConfigurationSourcing;
using Stronk.PropertySelection;
using Stronk.ValueConversion;

namespace Stronk
{
	public class PropertyConversionUnit
	{
		public PropertyDescriptor Property { get; set; }
		public IValueConverter[] Converters { get; set; }
		public string Value { get; set; }
		public IConfigurationSource Source { get; set; }
	}
}
