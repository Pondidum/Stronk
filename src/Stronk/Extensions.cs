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
				.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			var appSettings = ConfigurationManager.AppSettings;

			foreach (var property in properties)
			{
				var hasSetting = appSettings.AllKeys.Contains(property.Name);

				if (hasSetting)
				{
					var converted = converters
						.First(c => c.CanMap(property.PropertyType))
						.Map(property.PropertyType, appSettings[property.Name]);

					property.GetSetMethod(true).Invoke(target, new[] { converted });
				}
			}
		}
	}
}
