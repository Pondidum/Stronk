using System.Collections.Generic;

namespace Stronk.PropertyWriters
{
	public interface IPropertyWriter
	{
		IEnumerable<PropertyDescriptor> Select(PropertyWriterArgs args);
	}
}
