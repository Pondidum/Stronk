using System;
using System.Collections.Generic;
using NSubstitute;
using Shouldly;
using Stronk.ConfigurationSources;
using Stronk.PropertyMappers;
using Stronk.PropertyWriters;
using Stronk.Validation;
using Xunit;

namespace Stronk.Tests.Validation
{
	public class ValueSelectorValidatorTests
	{
		private readonly ValueSelectorValidator _validator;
		private Dictionary<string, string> _settings;
		private ValueSelector _selector;

		public ValueSelectorValidatorTests()
		{
			_validator = new ValueSelectorValidator();
			_settings = new Dictionary<string, string>();

			var config = Substitute.For<IStronkConfig>();
			config.ConfigSources.Returns(new[] { new DictionarySource(_settings) });
			config.Mappers.Returns(new[] { new PropertyNamePropertyMapper() });

			_selector = new ValueSelector(config);
		}

		[Fact]
		public void CanValidate_allows_ValueSelector()
		{
			_validator.CanValidate<ValueSelector>().ShouldBeTrue();
		}

		[Fact]
		public void CanValidate_rejects_non_ValueSelector_arguments()
		{
			_validator.CanValidate<LambdaValidator>().ShouldBeFalse();
		}

		[Fact]
		public void When_validating_a_non_ValueSelector_it_just_returns()
		{
			var selector = new ValueSelector(Substitute.For<IStronkConfig>());

			Should.NotThrow(() => _validator.Validate(selector));
		}

		[Fact]
		public void When_the_ValueSelector_has_no_unused_source_values()
		{
			_settings["One"] = "17";
			_selector.Select(new TestDescriptor("One", typeof(int)));

			Should.NotThrow(() => _validator.Validate(_selector));
		}

		[Fact]
		public void When_the_ValueSelector_has_unused_source_values()
		{
			_settings["One"] = "17";
			_settings["Two"] = "62";
			_settings["Three"] = "94";
			_selector.Select(new TestDescriptor("Two", typeof(int)));

			var ex = Should.Throw<UnusedConfigurationEntriesException>(() => _validator.Validate(_selector));

			ex.ShouldSatisfyAllConditions(
				() => ex.UnusedKeys.ShouldBe(new[] { "One", "Three" }, ignoreOrder: true),
				() => ex.Message.ShouldContain("* One"),
				() => ex.Message.ShouldContain("* Three")
			);
		}

		private class TestDescriptor : PropertyDescriptor
		{
			public TestDescriptor(string name, Type type) : base(name, type)
			{
			}

			public override void Assign(object target, object value)
			{
				throw new NotImplementedException();
			}
		}
	}
}
