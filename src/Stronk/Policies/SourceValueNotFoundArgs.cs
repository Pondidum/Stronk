using System.Collections.Generic;
using Stronk.ConfigurationSources;
using Stronk.PropertyMappers;
using Stronk.PropertyWriters;
using Stronk.ValueConverters;

namespace Stronk.Policies
{
	public class SourceValueNotFoundArgs
	{
		public IEnumerable<IPropertyMapper> ValueSelectors { get; set; }
		public PropertyDescriptor Property { get; set; }
		public IEnumerable<IConfigurationSource> Sources { get; set; }
	}
}
