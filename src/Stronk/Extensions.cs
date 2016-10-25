using System;
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

			var properties = target
				.GetType()
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Select(prop => new PropertyDescriptor
				{
					Name = prop.Name,
					Type = prop.PropertyType,
					Assign = value => prop.GetSetMethod(true).Invoke(target, new []{ value })
				});

			var appSettings = ConfigurationManager.AppSettings;

			foreach (var property in properties)
			{
				var hasSetting = appSettings.AllKeys.Contains(property.Name);

				if (hasSetting)
				{
					var converted = converters
						.First(c => c.CanMap(property.Type))
						.Map(property.Type, appSettings[property.Name]);

					property.Assign(converted);
				}
			}
		}
	}

	public class PropertyDescriptor
	{
		public string Name { get; set; }
		public Type Type { get; set; }
		public Action<object> Assign { get; set; }
	}
}
