using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Stronk.PropertySelection
{
	public class BackingFieldPropertySelector : IPropertySelector
	{
		private const BindingFlags PropertyBindingFlags = BindingFlags.Public | BindingFlags.Instance;
		private const BindingFlags FieldBindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase;

		public IEnumerable<PropertyDescriptor> Select(Type targetType)
		{
			var properties = targetType.GetProperties(PropertyBindingFlags);
			var descriptors = new List<PropertyDescriptor>(properties.Length);

			foreach (var property in properties)
			{
				var field = targetType.GetField("_" + property.Name, FieldBindingFlags);

				if (field == null)
					field = targetType.GetField(property.Name, FieldBindingFlags);

				if (field != null)
					descriptors.Add(new PropertyDescriptor
					{
						Name = property.Name,
						Type = property.PropertyType,
						Assign = (target, value) => field.SetValue(target, value)
					});

			}

			return descriptors;
		}
	}
}
