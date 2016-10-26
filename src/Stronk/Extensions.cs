using System;
using System.Configuration;
using System.Linq;
using Stronk.PropertySelection;
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
			var connectionStrings = ConfigurationManager.ConnectionStrings;

			foreach (var property in properties)
			{
				var value = appSettings[property.Name] ?? connectionStrings[property.Name]?.ConnectionString;
				
				if (value != null)
				{
					var converted = converters
						.First(c => c.CanMap(property.Type))
						.Map(property.Type, value);

					property.Assign(target, converted);
				}
			}
		}
	}
}
