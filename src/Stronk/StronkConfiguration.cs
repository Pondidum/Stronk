using System;
using System.Collections.Generic;
using Stronk.PropertySelection;
using Stronk.ValueConversion;
using Stronk.ValueSelection;

namespace Stronk
{
	public class StronkConfiguration : IStronkConfiguration
	{
		public IEnumerable<IValueConverter> ValueConverters => _valueConverters;
		public IEnumerable<IPropertySelector> PropertySelectors => _propertySelectors;
		public IEnumerable<IValueSelector> ValueSelectors => _valueSelectors;

		private readonly List<IValueConverter> _valueConverters;
		private readonly List<IPropertySelector> _propertySelectors;
		private readonly List<IValueSelector> _valueSelectors;

		public StronkConfiguration()
		{
			_valueConverters = new List<IValueConverter>
			{
				new LambdaValueConverter<Uri>(val => new Uri(val)),
				new EnumValueConverter(),
				new FallbackValueConverter()
			};

			_propertySelectors = new List<IPropertySelector>
			{
				new PrivateSetterPropertySelector(),
				new BackingFieldPropertySelector(),
			};

			_valueSelectors = new List<IValueSelector>
			{
				new PropertyNameValueSelector(),
			};
		}
	}
}
