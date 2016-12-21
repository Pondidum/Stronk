using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Stronk.PropertySelection
{
	public class PrivateSetterPropertySelector : IPropertySelector
	{
		public IEnumerable<PropertyDescriptor> Select(PropertySelectorArgs args)
		{
			return args
				.TargetType
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(prop => prop.CanWrite)
				.Select(prop => new PropertyDescriptor
				{
					Name = prop.Name,
					Type = prop.PropertyType,
					Assign = (target, value) => prop.GetSetMethod(true).Invoke(target, new[] { value })
				});
		}
	}
}
