using System;
using System.Collections.Generic;
using Shouldly;
using Stronk.ConfigurationSources;
using Stronk.Policies;
using Xunit;

namespace Stronk.Tests.Scenarios
{
	public class MissingValuesTests
	{
		private readonly IConfigurationSource _source;
		private readonly Dictionary<string, string> _settings;

		public MissingValuesTests()
		{
			_settings = new Dictionary<string, string>();
			_source = new DictionarySource(_settings);
		}

		[Fact]
		public void When_there_are_no_settings()
		{
			var ex = Should.Throw<SourceValueNotFoundException>(() => new StronkConfig().From.Source(_source).Build<Config>());

			ex.ShouldSatisfyAllConditions(
				() => ex.Message.ShouldContain("TestInt"),
				() => ex.Message.ShouldContain(typeof(int).Name),
				() => ex.Message.ShouldContain("PropertyName"),
				() => ex.Message.ShouldContain("There were no Settings")
			);
		}

		[Fact]
		public void When_there_are_settings()
		{
			_settings["SomethingElse"] = "omg";

			var ex = Should.Throw<SourceValueNotFoundException>(() => new StronkConfig().From.Source(_source).Build<Config>());

			ex.ShouldSatisfyAllConditions(
				() => ex.Message.ShouldContain("TestInt"),
				() => ex.Message.ShouldContain(typeof(int).Name),
				() => ex.Message.ShouldContain("PropertyName"),
				() => ex.Message.ShouldContain("SomethingElse")
			);
		}

		[Fact]
		public void When_there_is_no_value_for_a_nullable_property()
		{
			var config = new StronkConfig()
				.From.Source(_source)
				.Build<NullablePropertyConfig>();

			config.SomeValue.HasValue.ShouldBeFalse();
		}

		[Fact]
		public void When_there_is_a_value_for_a_nullable_property()
		{
			_settings["SomeValue"] = "17";

			var config = new StronkConfig()
				.From.Source(_source)
				.Build<NullablePropertyConfig>();

			config.SomeValue.HasValue.ShouldBeTrue();
			config.SomeValue.ShouldBe(17);
		}

		private class NullablePropertyConfig
		{
			public Nullable<int> SomeValue { get; private set; }
		}

		public class Config
		{
			public int TestInt { get; set; }
		}
	}
}
