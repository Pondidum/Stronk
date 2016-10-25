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
				new BackingFieldPropertySelector(), 
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
		IEnumerable<PropertyDescriptor> Select(Type targetType);
	}

	public class PrivateSetterPropertySelector : IPropertySelector
	{
		public IEnumerable<PropertyDescriptor> Select(Type targetType)
		{
			return targetType
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

	public class PropertyDescriptor
	{
		public string Name { get; set; }
		public Type Type { get; set; }
		public Action<object, object> Assign { get; set; }
	}
}
