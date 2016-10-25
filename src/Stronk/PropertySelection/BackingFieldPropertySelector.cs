using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Stronk.PropertySelection
{
	public class BackingFieldPropertySelector : IPropertySelector
	{
		public IEnumerable<PropertyDescriptor> Select(Type targetType)
		{
			var fields = targetType
				.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
				.ToArray();

			var properties = targetType
				.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach (var property in properties)
			{
				var field = fields.FirstOrDefault(f => f.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase))
				            ?? fields.FirstOrDefault(f => f.Name.Equals("_" + property.Name, StringComparison.OrdinalIgnoreCase));

				if (field != null)
					yield return new PropertyDescriptor
					{
						Name = property.Name,
						Type = property.PropertyType,
						Assign = (target, value) => field.SetValue(target, value)
					};

			}
		}
	}
}
