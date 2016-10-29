using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using Stronk.PropertySelection;
using Stronk.ValueConversion;
using Stronk.ValueSelection;

namespace Stronk
{
	public interface IStronkConfiguration
	{
		IEnumerable<IValueConverter> Converters { get; }
		IEnumerable<IPropertySelector> PropertySelectors { get; }
		IEnumerable<IValueSelector> ValueSelectors { get; }
	}

	public class StronkConfiguration : IStronkConfiguration
	{
		public IEnumerable<IValueConverter> Converters { get; }
		public IEnumerable<IPropertySelector> PropertySelectors { get; }
		public IEnumerable<IValueSelector> ValueSelectors { get; }

		public StronkConfiguration()
		{
			Converters = new IValueConverter[]
			{
				new LambdaValueConverter<Uri>(val => new Uri(val)),
				new EnumValueConverter(),
				new FallbackValueConverter()
			};

			PropertySelectors = new IPropertySelector[]
			{
				new PrivateSetterPropertySelector(),
				new BackingFieldPropertySelector(),
			};

			ValueSelectors = new IValueSelector[]
			{
				new PropertyNameValueSelector(),
			};
		}
	}

	public static class Extensions
	{
		public static void FromAppConfig(this object target)
		{
			target.FromAppConfig(new StronkConfiguration());
		}

		public static void FromAppConfig(this object target, IStronkConfiguration configuration)
		{
			var propertySelectors = configuration.PropertySelectors;
			var valueSelectors = configuration.ValueSelectors.ToArray();
			var converters = configuration.Converters.ToArray();

			var properties = propertySelectors
				.SelectMany(selector => selector.Select(target.GetType()));

			var args = new ValueSelectorArgs(ConfigurationManager.AppSettings, ConfigurationManager.ConnectionStrings);

			foreach (var property in properties)
			{
				var value = valueSelectors
					.Select(filter => filter.Select(args.With(property)))
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
}
