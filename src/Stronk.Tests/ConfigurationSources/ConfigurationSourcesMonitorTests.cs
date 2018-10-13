using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Stronk.ConfigurationSources;
using Xunit;

namespace Stronk.Tests.ConfigurationSources
{
	public class ConfigurationSourcesMonitorTests
	{
		private static ConfigurationSourcesMonitor Create(params Dictionary<string, string>[] settings)
			=> new ConfigurationSourcesMonitor(settings.Select(set => new DictionarySource(set)));

		private string Query(ConfigurationSourcesMonitor monitor, string key) => monitor
			.Select(source => source.GetValue(key))
			.FirstOrDefault(value => value != null);

		[Fact]
		public void When_there_are_no_sources()
		{
			var monitor = Create();
			Query(monitor, "what");

			monitor.GetUnusedKeys().ShouldBeEmpty();
		}

		[Fact]
		public void When_one_source_has_all_values_used()
		{
			var monitor = Create(new Dictionary<string, string>
			{
				{ "One", "11" },
				{ "Two", "22" }
			});

			Query(monitor, "One");
			Query(monitor, "Two");

			monitor.GetUnusedKeys().ShouldBeEmpty();
		}

		[Fact]
		public void When_one_source_has_an_unused_value()
		{
			var monitor = Create(new Dictionary<string, string>
			{
				{ "One", "11" },
				{ "Two", "22" }
			});

			Query(monitor, "One");

			monitor.GetUnusedKeys().ShouldBe(new[] { "Two" });
		}

		[Fact]
		public void When_one_source_has_a_non_existing_value_used()
		{
			var monitor = Create(new Dictionary<string, string>
			{
				{ "One", "11" },
				{ "Two", "22" }
			});

			Query(monitor, "Wat");

			monitor.GetUnusedKeys().ShouldBe(new[] { "One", "Two" }, ignoreOrder: true);
		}

		[Fact]
		public void When_two_source_have_all_values_used()
		{
			var monitor = Create(
				new Dictionary<string, string>
				{
					{ "First_one", "11" },
					{ "First_two", "22" }
				},
				new Dictionary<string, string>
				{
					{ "Second_one", "33" },
					{ "Second_two", "44" }
				});

			Query(monitor, "First_one");
			Query(monitor, "First_two");
			Query(monitor, "Second_one");
			Query(monitor, "Second_two");

			monitor.GetUnusedKeys().ShouldBeEmpty();
		}

		[Fact]
		public void When_two_sources_and_one_has_an_unused_value()
		{
			var monitor = Create(
				new Dictionary<string, string>
				{
					{ "First_one", "11" },
					{ "First_two", "22" }
				},
				new Dictionary<string, string>
				{
					{ "Second_one", "33" },
					{ "Second_two", "44" }
				});

			Query(monitor, "First_one");
			Query(monitor, "First_two");
			Query(monitor, "Second_two");

			monitor.GetUnusedKeys().ShouldBe(new[] { "Second_one" }, ignoreOrder: true);
		}

		[Fact]
		public void When_two_sources_with_one_identical_key_is_used()
		{
			var monitor = Create(
				new Dictionary<string, string>
				{
					{ "Same", "11" },
				},
				new Dictionary<string, string>
				{
					{ "Same", "33" },
				});

			Query(monitor, "Same");

			monitor.GetUnusedKeys().ShouldBeEmpty();
		}

		[Fact]
		public void When_two_sources_with_one_identical_key_with_differing_cases_are_used()
		{
			var monitor = Create(
				new Dictionary<string, string>
				{
					{ "same", "11" },
				},
				new Dictionary<string, string>
				{
					{ "SAME", "33" },
				});

			Query(monitor, "saME");

			monitor.GetUnusedKeys().ShouldBeEmpty();
		}
	}
}
