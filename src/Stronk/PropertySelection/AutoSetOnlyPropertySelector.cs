using System;
using System.Collections.Generic;

namespace Stronk.PropertySelection
{
	public class AutoSetOnlyPropertySelector : IPropertySelector
	{
		public IEnumerable<PropertyDescriptor> Select(Type targetType)
		{
			throw new NotImplementedException();
		}
	}
}
