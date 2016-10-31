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
				new LambdaValueConverter<Guid>(Guid.Parse),
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

		public void Add(IValueConverter valueConverter) => _valueConverters.Add(valueConverter);
		public void Add(IPropertySelector propertySelector) => _propertySelectors.Add(propertySelector);
		public void Add(IValueSelector valueSelector) => _valueSelectors.Add(valueSelector);

		public void AddBefore<T>(IValueConverter valueConverter) => InsertBefore(_valueConverters, typeof(T), valueConverter);
		public void AddBefore<T>(IPropertySelector propertySelector) => InsertBefore(_propertySelectors, typeof(T), propertySelector);
		public void AddBefore<T>(IValueSelector valueSelector) => InsertBefore(_valueSelectors, typeof(T), valueSelector);

		public void AddAfter<T>(IValueConverter valueConverter) => InsertAfter(_valueConverters, typeof(T), valueConverter);
		public void AddAfter<T>(IPropertySelector propertySelector) => InsertAfter(_propertySelectors, typeof(T), propertySelector);
		public void AddAfter<T>(IValueSelector valueSelector) => InsertAfter(_valueSelectors, typeof(T), valueSelector);

		private void InsertBefore<T>(List<T> collection, Type target, T value)
		{
			var index = collection.FindIndex(vc => vc.GetType() == target);

			if (index < 0)
				throw new StronkConfigurationException($"Unable to find a '{target.Name}' to add before");

			collection.Insert(index, value);
		}

		private void InsertAfter<T>(List<T> collection, Type target, T value)
		{
			var index = collection.FindIndex(vc => vc.GetType() == target);

			if (index < 0)
				throw new StronkConfigurationException($"Unable to find a '{target.Name}' to add before");

			collection.Insert(index + 1, value);
		}
	}
}
