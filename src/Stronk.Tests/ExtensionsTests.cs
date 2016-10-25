using System;
using System.Configuration;
using Shouldly;
using Xunit;

namespace Stronk.Tests
{
	public class ExtensionsTests
	{
		[Fact]
		public void When_loading_the_configuration_to_private_setters()
		{
			var config = new ConfigWithPrivateSetters();
			config.FromAppConfig();

			config.ShouldSatisfyAllConditions(
				() => config.Name.ShouldBe(ConfigurationManager.AppSettings["Name"]),
				() => config.Version.ShouldBe(Convert.ToInt32(ConfigurationManager.AppSettings["Version"])),
				() => config.Environment.ShouldBe((TargetEnvironment)Enum.Parse(typeof(TargetEnvironment), ConfigurationManager.AppSettings["Environment"], true)),
				() => config.Endpoint.ShouldBe(new Uri(ConfigurationManager.AppSettings["Endpoint"]))
			);
		}

		[Fact]
		public void When_loading_the_configuration_to_backing_fields()
		{
			var config = new ConfigWithBackingFields();
			config.FromAppConfig();

			config.ShouldSatisfyAllConditions(
				() => config.Name.ShouldBe(ConfigurationManager.AppSettings["Name"]),
				() => config.Version.ShouldBe(Convert.ToInt32(ConfigurationManager.AppSettings["Version"])),
				() => config.Environment.ShouldBe((TargetEnvironment)Enum.Parse(typeof(TargetEnvironment), ConfigurationManager.AppSettings["Environment"], true)),
				() => config.Endpoint.ShouldBe(new Uri(ConfigurationManager.AppSettings["Endpoint"]))
			);
		}

		public class ConfigWithPrivateSetters
		{
			public string Name { get; private set; }
			public int Version { get; private set; }
			public Uri Endpoint { get; private set; }
			public TargetEnvironment Environment { get; private set; }
		}

		public class ConfigWithBackingFields
		{
			private string _name;
			private int _version;
			private Uri _endpoint;
			private TargetEnvironment _environment;

			public string Name => _name;
			public int Version => _version;
			public Uri Endpoint => _endpoint;
			public TargetEnvironment Environment => _environment;
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
