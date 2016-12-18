using System;
using System.Collections.Generic;
using Stronk.PropertySelection;
using Stronk.SourceValueSelection;
using Stronk.ValueConversion;

namespace Stronk
{
	public static class Extensions
	{
		public static void FromAppConfig(this object target, StronkOptions options = null, IConfigurationSource configSource = null)
		{
			var builder = new ConfigBuilder(options ?? new StronkOptions());

			builder.Populate(target, configSource ?? new AppConfigSource());
		}

		public static void FromWebConfig(this object target, StronkOptions options = null, IConfigurationSource configSource = null)
		{
			var builder = new ConfigBuilder(options ?? new StronkOptions());

			builder.Populate(target, configSource ?? new AppConfigSource());
		}

		public static void AddBefore<T>(this List<IValueConverter> collection, IValueConverter valueConverter) => InsertBefore(collection, typeof(T), valueConverter);
		public static void AddBefore<T>(this List<IPropertySelector> collection, IPropertySelector propertySelector) => InsertBefore(collection, typeof(T), propertySelector);
		public static void AddBefore<T>(this List<ISourceValueSelector> collection, ISourceValueSelector sourceValueSelector) => InsertBefore(collection, typeof(T), sourceValueSelector);

		public static void AddAfter<T>(this List<IValueConverter> collection, IValueConverter valueConverter) => InsertAfter(collection, typeof(T), valueConverter);
		public static void AddAfter<T>(this List<IPropertySelector> collection, IPropertySelector propertySelector) => InsertAfter(collection, typeof(T), propertySelector);
		public static void AddAfter<T>(this List<ISourceValueSelector> collection, ISourceValueSelector sourceValueSelector) => InsertAfter(collection, typeof(T), sourceValueSelector);

		private static void InsertBefore<T>(List<T> collection, Type target, T value)
		{
			var index = collection.FindIndex(vc => vc.GetType() == target);

			if (index < 0)
				throw new StronkConfigurationException($"Unable to find a '{target.Name}' to add before");

			collection.Insert(index, value);
		}

		private static void InsertAfter<T>(List<T> collection, Type target, T value)
		{
			var index = collection.FindIndex(vc => vc.GetType() == target);

			if (index < 0)
				throw new StronkConfigurationException($"Unable to find a '{target.Name}' to add before");

			collection.Insert(index + 1, value);
		}
	}
}
