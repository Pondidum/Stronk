using System;
using System.Configuration;
using Shouldly;
using Stronk.Policies;
using Xunit;
#pragma warning disable 649

namespace Stronk.Tests
{
	public class AcceptanceTests
	{
		[Fact]
		public void When_loading_the_configuration_to_private_setters()
		{
			var config = new ConfigWithPrivateSetters();
			config.FromAppConfig();


			var appSettings = ConfigurationManager.AppSettings;
			var connectionStrings = ConfigurationManager.ConnectionStrings;

			config.ShouldSatisfyAllConditions(
				() => config.Name.ShouldBe(appSettings["Name"]),
				() => config.Version.ShouldBe(Convert.ToInt32(appSettings["Version"])),
				() => config.Environment.ShouldBe((TargetEnvironment)Enum.Parse(typeof(TargetEnvironment), appSettings["Environment"], true)),
				() => config.Endpoint.ShouldBe(new Uri(appSettings["Endpoint"])),
				() => config.DefaultDB.ShouldBe(connectionStrings["DefaultDB"].ConnectionString)
			);
		}

		[Fact]
		public void When_loading_the_configuration_to_backing_fields()
		{
			var config = new ConfigWithBackingFields();
			config.FromAppConfig();

			var appSettings = ConfigurationManager.AppSettings;
			var connectionStrings = ConfigurationManager.ConnectionStrings;

			config.ShouldSatisfyAllConditions(
				() => config.Name.ShouldBe(appSettings["Name"]),
				() => config.Version.ShouldBe(Convert.ToInt32(appSettings["Version"])),
				() => config.Environment.ShouldBe((TargetEnvironment)Enum.Parse(typeof(TargetEnvironment), appSettings["Environment"], true)),
				() => config.Endpoint.ShouldBe(new Uri(appSettings["Endpoint"])),
				() => config.DefaultDB.ShouldBe(connectionStrings["DefaultDB"].ConnectionString)
			);
		}

		[Fact]
		public void When_loading_the_configuration_and_a_property_throws()
		{
			var config = new ThrowingSetter();
			Should.Throw<ValueConversionException>(() => config.FromAppConfig());
		}

		public class ThrowingSetter
		{
			private string _name;

			public string Name
			{
				get { return _name; }
				set { throw new NotFiniteNumberException(); }
			}
		}
		
		public class ConfigWithPrivateSetters
		{
			public string Name { get; private set; }
			public int Version { get; private set; }
			public Uri Endpoint { get; private set; }
			public TargetEnvironment Environment { get; private set; }
			public string DefaultDB { get; private set; }
		}

		public class ConfigWithBackingFields
		{
			private string _name;
			private int _version;
			private Uri _endpoint;
			private TargetEnvironment _environment;
			private string _defaultDB;

			public string Name => _name;
			public int Version => _version;
			public Uri Endpoint => _endpoint;
			public TargetEnvironment Environment => _environment;
			public string DefaultDB => _defaultDB;
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
