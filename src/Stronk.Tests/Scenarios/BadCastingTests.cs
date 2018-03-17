using NSubstitute;
using Shouldly;
using Stronk.ConfigurationSourcing;
using Stronk.Policies;
using Xunit;

namespace Stronk.Tests.Scenarios
{
	public class BadCastingTests
	{
		private readonly IConfigurationSource _source;

		public BadCastingTests()
		{
			_source = Substitute.For<IConfigurationSource>();
		}

		[Fact]
		public void When_castsing_an_invalid_string_to_integer()
		{
			_source.GetValue("TestInt").Returns("am no an integer");

			var ex = Should.Throw<ValueConversionException>(() => new StronkConfig().From.Source(_source).Build<Config>());

			ex.ShouldSatisfyAllConditions(
				() => ex.Message.ShouldContain("TestInt"),
				() => ex.Message.ShouldContain("am no an integer"),
				() => ex.Message.ShouldContain(typeof(int).Name)
			);
		}

		public class Config
		{
			public int TestInt { get; set; }
		}
	}
}
