using System.Collections.Generic;

namespace Stronk.PropertySelection
{
	public interface IPropertySelector
	{
		IEnumerable<PropertyDescriptor> Select(PropertySelectorArgs args);
	}
}
