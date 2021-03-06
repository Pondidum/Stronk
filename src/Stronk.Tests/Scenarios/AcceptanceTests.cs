﻿using System;
using System.Collections.Generic;
using System.Configuration;
using Shouldly;
using Stronk.ConfigurationSources;
using Stronk.Validation;
using Xunit;

#pragma warning disable 649

namespace Stronk.Tests.Scenarios
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
		public void When_loading_and_checking_all_sources_are_used()
		{
			var settings = new Dictionary<string, string>
			{
				{ "One", "1" },
				{ "Two", "2" }
			};

			var ex = Should.Throw<UnusedConfigurationEntriesException>(() => new StronkConfig()
				.Validate.AllSourceValuesAreUsed()
				.From.Source(new DictionarySource(settings))
				.Build<OneProperty>());

			ex.UnusedKeys.ShouldBe(new[] { "Two" });
		}

		[Fact]
		public void When_loading_the_configuration_and_a_property_throws()
		{
			var config = new ThrowingSetter();
			Should.Throw<AggregateException>(() => config.FromAppConfig())
				.InnerExceptions
				.ShouldHaveSingleItem()
				.ShouldBeOfType<ExpectedException>();
		}

		public class ThrowingSetter
		{
			public string Name
			{
				get { return Guid.NewGuid().ToString(); }
				set { throw new ExpectedException(); }
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

		private class ExpectedException : Exception
		{
		}

		private class OneProperty
		{
			public int One { get; set; }
		}
	}
}
