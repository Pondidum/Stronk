using System;
using System.Collections.Generic;

namespace Stronk.PropertySelection
{
	public interface IPropertySelector
	{
		IEnumerable<PropertyDescriptor> Select(Type targetType);
	}
}
