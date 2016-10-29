using System;
using System.Collections.Generic;
using Stronk.PropertySelection;
using Stronk.ValueConversion;
using Stronk.ValueSelection;

namespace Stronk
{
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
}
