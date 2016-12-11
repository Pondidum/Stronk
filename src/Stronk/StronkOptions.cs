using System;
using System.Collections.Generic;
using System.Linq;
using Stronk.Policies;
using Stronk.PropertySelection;
using Stronk.SourceValueSelection;
using Stronk.ValueConversion;

namespace Stronk
{
	public class StronkOptions : IStronkOptions
	{
		public IEnumerable<IValueConverter> ValueConverters => _valueConverters;
		public IEnumerable<IPropertySelector> PropertySelectors => _propertySelectors;
		public IEnumerable<ISourceValueSelector> ValueSelectors => _valueSelectors;
		public ErrorPolicy ErrorPolicy { get; set; }

		private readonly List<IValueConverter> _valueConverters;
		private readonly List<IPropertySelector> _propertySelectors;
		private readonly List<ISourceValueSelector> _valueSelectors;

		public StronkOptions()
		{
			_valueConverters = Default.ValueConverters.ToList();
			_propertySelectors = Default.PropertySelectors.ToList();
			_valueSelectors = Default.SourceValueSelectors.ToList();

			ErrorPolicy = new ErrorPolicy();
		}

		public void Add(IValueConverter valueConverter) => _valueConverters.Add(valueConverter);
		public void Add(IPropertySelector propertySelector) => _propertySelectors.Add(propertySelector);
		public void Add(ISourceValueSelector sourceValueSelector) => _valueSelectors.Add(sourceValueSelector);

		public void AddBefore<T>(IValueConverter valueConverter) => InsertBefore(_valueConverters, typeof(T), valueConverter);
		public void AddBefore<T>(IPropertySelector propertySelector) => InsertBefore(_propertySelectors, typeof(T), propertySelector);
		public void AddBefore<T>(ISourceValueSelector sourceValueSelector) => InsertBefore(_valueSelectors, typeof(T), sourceValueSelector);

		public void AddAfter<T>(IValueConverter valueConverter) => InsertAfter(_valueConverters, typeof(T), valueConverter);
		public void AddAfter<T>(IPropertySelector propertySelector) => InsertAfter(_propertySelectors, typeof(T), propertySelector);
		public void AddAfter<T>(ISourceValueSelector sourceValueSelector) => InsertAfter(_valueSelectors, typeof(T), sourceValueSelector);

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
