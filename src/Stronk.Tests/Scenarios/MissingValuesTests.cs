using System.Collections.Generic;
using Shouldly;
using Stronk.ConfigurationSourcing;
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

		public class Config
		{
			public int TestInt { get; set; }
		}
	}
}
