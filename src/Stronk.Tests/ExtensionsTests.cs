using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Stronk.PropertySelection;
using Stronk.SourceValueSelection;
using Stronk.ValueConversion;
using Xunit;

namespace Stronk.Tests
{
	public class ExtensionsTests
	{
		private readonly List<IValueConverter> _valueConverters;
		private readonly List<ISourceValueSelector> _valueSelectors;
		private readonly List<IPropertySelector> _propertySelectors;

		public ExtensionsTests()
		{
			_valueConverters = Default.ValueConverters.ToList();
			_valueSelectors = Default.SourceValueSelectors.ToList();
			_propertySelectors = Default.PropertySelectors.ToList();
		}
		
		[Fact]
		public void When_a_value_converter_is_added()
		{
			_valueConverters.Add(new DtoValueConverter());

			_valueConverters.Last().ShouldBeOfType<DtoValueConverter>();
		}

		[Fact]
		public void When_selecting_type_names()
		{
			var types = new[]
			{
				typeof(LambdaValueConverter<Uri>),
				typeof(LambdaValueConverter<Guid>),
				typeof(CsvValueConverter),
				typeof(LambdaValueConverter<List<Dictionary<string, int>>>)
			};

			types.SelectTypeNames().ShouldBe(new []
			{
				"LambdaValueConverter<Uri>",
				"LambdaValueConverter<Guid>",
				"CsvValueConverter",
				"LambdaValueConverter<List<Dictionary<String, Int32>>>"
			});
		}



		private static void InsertIndexShouldBeBefore<T>(IEnumerable<T> collection, Type search, Type inserted)
		{
			var converters = collection.Select(c => c.GetType()).ToList();
			var searchIndex = converters.IndexOf(search);
			var insertIndex = converters.IndexOf(inserted);

			insertIndex.ShouldBe(searchIndex - 1);
		}

		private static void InsertIndexShouldBeAfter<T>(IEnumerable<T> collection, Type search, Type inserted)
		{
			var converters = collection.Select(c => c.GetType()).ToList();
			var searchIndex = converters.IndexOf(search);
			var insertIndex = converters.IndexOf(inserted);

			insertIndex.ShouldBe(searchIndex + 1);
		}

		private class Dto { }

		private class DtoValueConverter : IValueConverter
		{
			public bool CanMap(Type target) => target == typeof(Dto);
			public object Map(ValueConverterArgs e) => new Dto();
		}

		private class DtoSourceValueSelector : ISourceValueSelector
		{
			public string Select(ValueSelectorArgs args) => "dto";
		}

		private class DtoPropertySelector : IPropertySelector
		{
			public IEnumerable<PropertyDescriptor> Select(PropertySelectorArgs args) => Enumerable.Empty<PropertyDescriptor>();
		}

		private class UnusedSourceValueSelector : ISourceValueSelector
		{
			public string Select(ValueSelectorArgs args)
			{
				throw new NotSupportedException();
			}
		}

		private class UnusedPropertySelector : IPropertySelector
		{
			public IEnumerable<PropertyDescriptor> Select(PropertySelectorArgs args)
			{
				throw new NotSupportedException();
			}
		}
	}
}
