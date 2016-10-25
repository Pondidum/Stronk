using System;
using System.Configuration;
using Shouldly;
using Xunit;

namespace Stronk.Tests
{
	public class ExtensionsTests
	{
		[Fact]
		public void When_loading_the_configuration()
		{
			var config = new Config();
			config.FromAppConfig();

			config.ShouldSatisfyAllConditions(
				() => config.Name.ShouldBe(ConfigurationManager.AppSettings["Name"]),
				() => config.Version.ShouldBe(Convert.ToInt32(ConfigurationManager.AppSettings["Version"])),
				() => config.Environment.ShouldBe((TargetEnvironment)Enum.Parse(typeof(TargetEnvironment), ConfigurationManager.AppSettings["Environment"], true)),
				() => config.Endpoint.ShouldBe(new Uri(ConfigurationManager.AppSettings["Endpoint"]))
			);
		}

		public class Config
		{
			public string Name { get; private set; }
			public int Version { get; private set; }
			public Uri Endpoint { get; private set; }
			public TargetEnvironment Environment { get; private set; }
		}

		public enum TargetEnvironment
		{
			Dev,
			Test,
			ExternalTest,
			QA,
			Production
		}
	}
}
