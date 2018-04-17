using System;
using System.Collections.Generic;
using Shouldly;
using Stronk.ValueConverters;
using Xunit;

namespace Stronk.Tests.ValueConverters
{
	public class CsvValueConverterTests
	{
		private readonly CsvValueConverter _converter;

		public CsvValueConverterTests()
		{
			_converter = new CsvValueConverter();
		}

		[Fact]
		public void When_type_is_array_t()
		{
			_converter
				.CanMap(typeof(int[]))
				.ShouldBe(true);
		}

		[Theory]
		[InlineData(typeof(int[]), true)]
		[InlineData(typeof(IEnumerable<int>), true)]
		[InlineData(typeof(List<int>), true)]
		[InlineData(typeof(IList<int>), true)]
		[InlineData(typeof(string), false)]
		public void It_can_map_correct_types(Type type, bool expected)
		{
			if (expected)
				_converter.CanMap(type).ShouldBeTrue();
			else
				_converter.CanMap(type).ShouldBeFalse();
		}

		[Fact]
		public void When_mapping_int_csv_to_ienumerable()
		{
			_converter
				.Map(Create<IList<int>>("1,2,3,4"))
				.ShouldBe(new[] { 1, 2, 3, 4 });
		}

		[Fact]
		public void When_mapping_int_csv_to_array()
		{
			_converter
				.Map(Create<int[]>("1,2,3,4"))
				.ShouldBe(new[] { 1, 2, 3, 4 });
		}

		[Fact]
		public void When_mapping_int_csv_to_ilist()
		{
			_converter
				.Map(Create<IList<int>>("1,2,3,4"))
				.ShouldBe(new List<int> { 1, 2, 3, 4 });
		}

		[Fact]
		public void When_mapping_a_type_supported_by_another_converter()
		{
			var converters = new IValueConverter[]
			{
				new LambdaValueConverter<Guid>(Guid.Parse),
				new FallbackValueConverter()
			};

			var guids = new[]
			{
				Guid.NewGuid(),
				Guid.NewGuid(),
				Guid.NewGuid()
			};

			_converter
				.Map(Create<IEnumerable<Guid>>(string.Join(",", guids), converters))
				.ShouldBe(guids);
		}

		private ValueConverterArgs Create<T>(string value, IEnumerable<IValueConverter> others = null)
		{
			return new ValueConverterArgs(
				(message, args) => { },
				others ?? new[] { new FallbackValueConverter() },
				typeof(T),
				value);
		}
	}
}
