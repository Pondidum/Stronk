using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Stronk.PropertyWriters
{
	public class PrivateSetterPropertyWriter : IPropertyWriter
	{
		public IEnumerable<PropertyDescriptor> Select(PropertyWriterArgs args)
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
