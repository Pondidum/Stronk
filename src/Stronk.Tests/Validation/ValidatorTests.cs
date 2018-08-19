using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Stronk.Tests.TestUtils;
using Stronk.Validation;
using Xunit;

namespace Stronk.Tests.Validation
{
	public class ValidatorTests
	{
		private static Validator Create(params IValidator[] validators)
		{
			var config = Substitute.For<IStronkConfig>();
			config.Validators.Returns(validators);

			return new Validator(config);
		}

		[Fact]
		public void When_there_are_no_validators()
		{
			var validator = Create();

			Should.NotThrow(() => validator.Validate(new Target()));
		}

		[Fact]
		public void When_there_is_one_non_applicable_validator()
		{
			var validator = Substitute.For<IValidator>();
			validator.CanValidate<Target>().Returns(false);
			validator.When(x => x.Validate(Arg.Any<Target>())).Do(ci => throw new ExpectedException());

			var target = new Target();
			Create(validator).Validate(target);

			validator.DidNotReceive().Validate(target);
		}

		[Fact]
		public void When_one_validator_passes()
		{
			var validator = Substitute.For<IValidator>();
			validator.CanValidate<Target>().Returns(true);

			var target = new Target();
			Create(validator).Validate(target);
		}

		[Fact]
		public void When_one_validator_fails()
		{
			var validator = Substitute.For<IValidator>();
			validator.CanValidate<Target>().Returns(true);
			validator.When(x => x.Validate(Arg.Any<Target>())).Do(ci => throw new ExpectedException());

			var sut = Create(validator);

			Should.Throw<ExpectedException>(() => sut.Validate(new Target()));
		}

		private class Target
		{
			public int Value { get; set; }
		}
	}
}
