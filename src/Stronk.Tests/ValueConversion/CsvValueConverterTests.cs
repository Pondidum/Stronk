using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Stronk.ValueConversion;
using Xunit;

namespace Stronk.Tests.ValueConversion
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

		[Fact]
		public void When_type_is_ienumerable_t()
		{
			_converter
				.CanMap(typeof(IEnumerable<int>))
				.ShouldBe(true);
		}

		[Fact]
		public void When_type_is_list_t()
		{
			_converter
				.CanMap(typeof(List<int>))
				.ShouldBe(true);
		}

		[Fact]
		public void When_type_is_ilist_t()
		{
			_converter
				.CanMap(typeof(IList<int>))
				.ShouldBe(true);
		}

		[Fact]
		public void When_type_is_a_string()
		{
			_converter
				.CanMap(typeof(string))
				.ShouldBe(false);
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
				others ?? new[] { new FallbackValueConverter() },
				typeof(T),
				value);
		}
	}
}
