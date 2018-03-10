using System.Collections.Generic;
using Stronk.ConfigurationSourcing;
using Stronk.PropertyWriters;
using Stronk.SourceValueSelection;
using Stronk.ValueConversion;

namespace Stronk.Policies
{
	public class SourceValueNotFoundArgs
	{
		public IEnumerable<ISourceValueSelector> ValueSelectors { get; set; }
		public PropertyDescriptor Property { get; set; }
		public IValueConverter[] Converters { get; set; }
		public IEnumerable<IConfigurationSource> Sources { get; set; }
	}
}
