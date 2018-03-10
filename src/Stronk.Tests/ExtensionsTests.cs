using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Stronk.PropertyWriters;
using Stronk.SourceValueSelection;
using Stronk.ValueConversion;
using Xunit;

namespace Stronk.Tests
{
	public class ExtensionsTests
	{
		private readonly List<IValueConverter> _valueConverters;
		private readonly List<ISourceValueSelector> _valueSelectors;
		private readonly List<IPropertyWriter> _propertySelectors;

		public ExtensionsTests()
		{
			_valueConverters = Default.ValueConverters.ToList();
			_valueSelectors = Default.SourceValueSelectors.ToList();
			_propertySelectors = Default.PropertyWriters.ToList();
		}
		
		[Fact]
		public void When_a_value_converter_is_added()
		{
			_valueConverters.Add(new DtoValueConverter());

			_valueConverters.Last().ShouldBeOfType<DtoValueConverter>();
		}

		[Fact]
		public void When_selecting_type_names_of_instances()
		{
			var types = new IValueConverter[]
			{
				new LambdaValueConverter<Uri>(x => new Uri(x)),
				new LambdaValueConverter<Guid>(x => Guid.Parse(x)),
				new CsvValueConverter(),
				new LambdaValueConverter<List<Dictionary<string, int>>>(x => new List<Dictionary<string, int>>())
			};

			types.SelectTypeNames().ShouldBe(new []
			{
				"LambdaValueConverter<Uri>",
				"LambdaValueConverter<Guid>",
				"CsvValueConverter",
				"LambdaValueConverter<List<Dictionary<String, Int32>>>"
			});
		}

		[Fact]
		public void When_selecting_type_names_of_types()
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

		private class DtoPropertyWriter : IPropertyWriter
		{
			public IEnumerable<PropertyDescriptor> Select(PropertyWriterArgs args) => Enumerable.Empty<PropertyDescriptor>();
		}

		private class UnusedSourceValueSelector : ISourceValueSelector
		{
			public string Select(ValueSelectorArgs args)
			{
				throw new NotSupportedException();
			}
		}

		private class UnusedPropertyWriter : IPropertyWriter
		{
			public IEnumerable<PropertyDescriptor> Select(PropertyWriterArgs args)
			{
				throw new NotSupportedException();
			}
		}
	}
}
