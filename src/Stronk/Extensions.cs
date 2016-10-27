using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

			var valueSelectors = new IValueSelector[]
			{
				new PropertyNameValueSelector(),
			};

			var properties = propertySelectors
				.SelectMany(selector => selector.Select(target.GetType()));
				

			var appSettings = ConfigurationManager.AppSettings;
			var connectionStrings = ConfigurationManager.ConnectionStrings;

			foreach (var property in properties)
			{
				var value = valueSelectors
					.Select(filter => filter.Select(appSettings, connectionStrings, property))
					.Where(v => v != null)
					.DefaultIfEmpty(null)
					.First();

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

	public interface IValueSelector
	{
		string Select(NameValueCollection appSettings, ConnectionStringSettingsCollection connectionStrings, PropertyDescriptor property);
	}

	public class PropertyNameValueSelector : IValueSelector
	{
		public string Select(NameValueCollection appSettings, ConnectionStringSettingsCollection connectionStrings, PropertyDescriptor property)
		{
			return appSettings[property.Name] ?? connectionStrings[property.Name]?.ConnectionString;
		}
	}
}
