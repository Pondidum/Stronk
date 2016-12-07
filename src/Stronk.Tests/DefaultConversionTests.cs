using System;
using System.Linq;
using Shouldly;
using Stronk.ValueConversion;
using Xunit;

namespace Stronk.Tests
{
	public class DefaultConversionTests
	{
		private TTarget Convert<TTarget>(string sourceValue)
		{
			var allConverters = Default.ValueConverters;
			var converter = allConverters.First(c => c.CanMap(typeof(TTarget)));

			return (TTarget)converter.Map(new ValueConverterArgs(
				allConverters.Where(x => x != converter),
				typeof(TTarget),
				sourceValue));
		}

		[Fact]
		public void Strings_are_converted() => Convert<string>("hEllo").ShouldBe("hEllo");

		[Fact]
		public void Integers_are_converted() => Convert<int>("12").ShouldBe(12);

		[Fact]
		public void Decimals_are_converted() => Convert<decimal>("12.453").ShouldBe(12.453M);

		[Fact]
		public void Doubles_are_converted() => Convert<double>("12.453").ShouldBe(12.453);

		[Fact]
		public void Guids_are_converted() => Convert<Guid>("7AB987EE-0ACE-45FF-B6CA-DE75091D045D").ShouldBe(Guid.Parse("7AB987EE-0ACE-45FF-B6CA-DE75091D045D"));

		[Fact]
		public void Urls_are_converted() => Convert<Uri>("https://example.com/123").ShouldBe(new Uri("https://example.com/123"));

		[Fact]
		public void Enum_text_is_converted() => Convert<Enum>("Two").ShouldBe(Enum.Two);

		[Fact]
		public void Enum_value_is_converted() => Convert<Enum>("3").ShouldBe(Enum.Three);



		[Fact]
		public void String_arrays_are_converted() => Convert<string[]>("a,b,c,d").ShouldBe(new[] { "a", "b", "c", "d" });

		[Fact]
		public void Integer_arrays_are_converted() => Convert<int[]>("1,3,5,9").ShouldBe(new[] { 1, 3, 5, 9 });

		[Fact]
		public void Decimal_arrays_are_converted() => Convert<decimal[]>("1.2,3.4,5.6,9.871").ShouldBe(new[] { 1.2M, 3.4M, 5.6M, 9.871M });

		[Fact]
		public void Guid_arrays_are_converted() => Convert<Guid[]>("7AB987EE-0ACE-45FF-B6CA-DE75091D045D,1D19B692-6115-41FF-AB78-7731E3BECE21").ShouldBe(new[]
		{
			Guid.Parse("7AB987EE-0ACE-45FF-B6CA-DE75091D045D"),
			Guid.Parse("1D19B692-6115-41FF-AB78-7731E3BECE21")
		});

		[Fact]
		public void Url_arrays_are_converted() => Convert<Uri[]>("https://example.com/123,http://www.example.org").ShouldBe(new[]
		{
			new Uri("https://example.com/123"),
			new Uri("http://www.example.org")
		});

		[Fact]
		public void Enum_text_arrays_are_converted() => Convert<Enum[]>("One,Two").ShouldBe(new[] { Enum.One, Enum.Two });

		[Fact]
		public void Enum_value_arrays_are_is_converted() => Convert<Enum[]>("2,3").ShouldBe(new[] { Enum.Two, Enum.Three });

		private enum Enum
		{
			Zero,
			One,
			Two,
			Three
		}
	}
}
