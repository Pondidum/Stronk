using System.Collections.Generic;
using System.Configuration;
using NSubstitute;
using Shouldly;
using Stronk.ConfigurationSourcing;
using Stronk.Policies;
using Xunit;

namespace Stronk.Tests.Scenarios
{
	public class MissingValuesTests
	{
		private readonly IConfigurationSource _source;

		public MissingValuesTests()
		{
			_source = Substitute.For<IConfigurationSource>();
			_source.AppSettings.Returns(new Dictionary<string, string>());
			_source.ConnectionStrings.Returns(new Dictionary<string, ConnectionStringSettings>());
		}

		[Fact]
		public void When_there_are_no_settings()
		{
			var ex = Should.Throw<SourceValueNotFoundException>(() => new Config().FromAppConfig(configSource: _source));

			ex.ShouldSatisfyAllConditions(
				() => ex.Message.ShouldContain("TestInt"),
				() => ex.Message.ShouldContain(typeof(int).Name),
				() => ex.Message.ShouldContain("PropertyName"),
				() => ex.Message.ShouldContain("There were no AppSettings or ConnectionStrings")
			);
		}

		[Fact]
		public void When_there_are_appsettings()
		{
			_source.AppSettings["SomethingElse"] = "omg";

			var ex = Should.Throw<SourceValueNotFoundException>(() => new Config().FromAppConfig(configSource: _source));

			ex.ShouldSatisfyAllConditions(
				() => ex.Message.ShouldContain("TestInt"),
				() => ex.Message.ShouldContain(typeof(int).Name),
				() => ex.Message.ShouldContain("PropertyName"),
				() => ex.Message.ShouldContain("AppSettings:"),
				() => ex.Message.ShouldContain("SomethingElse")
			);
		}

		[Fact]
		public void When_there_are_connectionstrings()
		{
			_source.ConnectionStrings["MainDB"] = new ConnectionStringSettings();

			var ex = Should.Throw<SourceValueNotFoundException>(() => new Config().FromAppConfig(configSource: _source));

			ex.ShouldSatisfyAllConditions(
				() => ex.Message.ShouldContain("TestInt"),
				() => ex.Message.ShouldContain(typeof(int).Name),
				() => ex.Message.ShouldContain("PropertyName"),
				() => ex.Message.ShouldContain("ConnectionStrings:"),
				() => ex.Message.ShouldContain("MainDB")
			);
		}

		public class Config
		{
			public int TestInt { get; set; }
		}
	}
}
