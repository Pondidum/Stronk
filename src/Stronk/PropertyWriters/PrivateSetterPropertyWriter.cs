using System;
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
				.Select(prop => new PrivateSetterDescriptor(prop));
		}

		private class PrivateSetterDescriptor : PropertyDescriptor
		{
			private readonly PropertyInfo _property;

			public PrivateSetterDescriptor(PropertyInfo property)
				: base(property.Name, property.PropertyType)
			{
				_property = property;
			}

			public override void Assign(object target, object value)
			{
				_property.GetSetMethod(true).Invoke(target, new[] { value });
			}
		}
	}
}
