using System;
using System.Linq;
using System.Net.Mail;
using Shouldly;
using Xunit;
using Stronk.ValueConversion;

namespace Stronk.Tests.ValueConversion
{
	public class EnumValueConverterTests
	{
		private readonly EnumValueConverter _converter;

		public EnumValueConverterTests()
		{
			_converter = new EnumValueConverter();
		}

		[Fact]
		public void When_type_type_is_not_an_enum()
		{
			_converter
				.CanMap(typeof(MailMessage))
				.ShouldBe(false);
		}

		[Fact]
		public void When_the_type_is_a_structure()
		{
			_converter
				.CanMap(typeof(Guid))
				.ShouldBe(false);
		}

		[Fact]
		public void When_the_type_is_an_enum()
		{
			_converter
				.CanMap(typeof(TestEnum))
				.ShouldBe(true);
		}

		[Fact]
		public void When_the_string_value_is_defined()
		{
			_converter
				.Map(Create("First"))
				.ShouldBe(TestEnum.First);
		}

		[Fact]
		public void When_the_string_value_is_defined_but_case_differs()
		{
			_converter
				.Map(Create("SECoND"))
				.ShouldBe(TestEnum.Second);
		}

		[Fact]
		public void When_the_string_value_is_not_defined()
		{
			_converter
				.Map(Create("omg_hai"))
				.ShouldBe(null);
		}

		[Fact]
		public void When_the_int_value_is_defined()
		{
			_converter
				.Map(Create("3"))
				.ShouldBe(TestEnum.Third);
		}

		[Fact]
		public void When_the_int_value_is_not_defined()
		{
			_converter
				.Map(Create("17"))
				.ShouldBe(null);
		}

		private ValueConverterArgs Create(string value)
		{
			return new ValueConverterArgs(
				Enumerable.Empty<IValueConverter>(),
				typeof(TestEnum),
				value);
		}

		private enum TestEnum
		{
			Zeroth,
			First,
			Second, 
			Third,
			Fourth
		}	
	}
}