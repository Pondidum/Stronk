using System.Collections.Generic;
using Stronk.PropertyWriters;
using Stronk.ValueConversion;

namespace Stronk.Policies
{
	public class ConverterNotFoundArgs
	{
		public IEnumerable<IValueConverter> AvailableConverters { get; set; }
		public PropertyDescriptor Property { get; set; }
	}
}
