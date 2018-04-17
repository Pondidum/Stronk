using System;
using System.Collections.Generic;
using Shouldly;
using Stronk.ValueConverters;
using Xunit;

namespace Stronk.Tests.ValueConverters
{
	public class NullableValueConverterTests
	{
		private readonly NullableValueConverter _converter;

		public NullableValueConverterTests()
		{
			_converter = new NullableValueConverter();
		}

		[Theory]
		[InlineData(typeof(int?), true)]
		[InlineData(typeof(int), false)]
		[InlineData(typeof(bool?), true)]
		[InlineData(typeof(bool), false)]
		public void It_can_map_correct_types(Type type, bool expected)
		{
			if (expected)
				_converter.CanMap(type).ShouldBeTrue();
			else
				_converter.CanMap(type).ShouldBeFalse();
		}

		[Fact]
		public void When_converting_a_value()
		{
			_converter
				.Map(Create<int?>("17"))
				.ShouldBe(17);
		}

		[Fact]
		public void When_converting_null()
		{
			_converter
				.Map(Create<int?>(""))
				.ShouldBe(null);
		}

		private static ValueConverterArgs Create<T>(string value, IEnumerable<IValueConverter> others = null) =>
			new ValueConverterArgs(
				(message, args) => { },
				others ?? new[] { new FallbackValueConverter() },
				typeof(T),
				value);
	}
}
