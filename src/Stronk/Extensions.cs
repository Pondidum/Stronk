using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Stronk.ValueConversion;

namespace Stronk
{
	public static class Extensions
	{
		public static void FromAppConfig(this object target)
		{
			var converters = new IValueConverter[]
			{
				new LambdaValueConverter<Uri>(val => new Uri(val)), 
				new EnumValueConverter(),
				new FallbackValueConverter()
			};

			var propertySelectors = new IPropertySelector[]
			{
				new PrivateSetterPropertySelector(),
			};

			var properties = propertySelectors
				.SelectMany(selector => selector.Select(target.GetType()));
				

			var appSettings = ConfigurationManager.AppSettings;

			foreach (var property in properties)
			{
				var hasSetting = appSettings.AllKeys.Contains(property.Name);

				if (hasSetting)
				{
					var converted = converters
						.First(c => c.CanMap(property.Type))
						.Map(property.Type, appSettings[property.Name]);

					property.Assign(target, converted);
				}
			}
		}
	}

	public interface IPropertySelector
	{
		IEnumerable<PropertyDescriptor> Select(Type target);
	}

	public class PrivateSetterPropertySelector : IPropertySelector
	{
		public IEnumerable<PropertyDescriptor> Select(Type target)
		{
			return target
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Select(prop => new PropertyDescriptor
				{
					Name = prop.Name,
					Type = prop.PropertyType,
					Assign = (targetObject, value) => prop.GetSetMethod(true).Invoke(targetObject, new[] { value })
				});
		}
	}

	public class PropertyDescriptor
	{
		public string Name { get; set; }
		public Type Type { get; set; }
		public Action<object, object> Assign { get; set; }
	}
}
