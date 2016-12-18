using System;
using System.Collections.Generic;
using System.Linq;
using Stronk.Policies;
using Stronk.PropertySelection;
using Stronk.SourceValueSelection;
using Stronk.ValueConversion;

namespace Stronk
{
	public class StronkOptions
	{
		public List<IValueConverter> ValueConverters { get; set; }
		public List<IPropertySelector> PropertySelectors { get; set; }
		public List<ISourceValueSelector> ValueSelectors { get; set; }
		public ErrorPolicy ErrorPolicy { get; set; }

		public StronkOptions()
		{
			ValueConverters = Default.ValueConverters.ToList();
			PropertySelectors = Default.PropertySelectors.ToList();
			ValueSelectors = Default.SourceValueSelectors.ToList();

			ErrorPolicy = new ErrorPolicy();
		}

		public void Add(IValueConverter valueConverter) => ValueConverters.Add(valueConverter);
		public void Add(IPropertySelector propertySelector) => PropertySelectors.Add(propertySelector);
		public void Add(ISourceValueSelector sourceValueSelector) => ValueSelectors.Add(sourceValueSelector);

		public void AddBefore<T>(IValueConverter valueConverter) => InsertBefore(ValueConverters, typeof(T), valueConverter);
		public void AddBefore<T>(IPropertySelector propertySelector) => InsertBefore(PropertySelectors, typeof(T), propertySelector);
		public void AddBefore<T>(ISourceValueSelector sourceValueSelector) => InsertBefore(ValueSelectors, typeof(T), sourceValueSelector);

		public void AddAfter<T>(IValueConverter valueConverter) => InsertAfter(ValueConverters, typeof(T), valueConverter);
		public void AddAfter<T>(IPropertySelector propertySelector) => InsertAfter(PropertySelectors, typeof(T), propertySelector);
		public void AddAfter<T>(ISourceValueSelector sourceValueSelector) => InsertAfter(ValueSelectors, typeof(T), sourceValueSelector);

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
