using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Stronk.ValueConversion;
using Xunit;

namespace Stronk.Tests
{
	public class ExtensionsTests
	{
		private readonly List<IValueConverter> _valueConverters;

		public ExtensionsTests()
		{
			_valueConverters = Default.ValueConverters.ToList();
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

		private class Dto { }

		private class DtoValueConverter : IValueConverter
		{
			public bool CanMap(Type target) => target == typeof(Dto);
			public object Map(ValueConverterArgs e) => new Dto();
		}
	}
}
