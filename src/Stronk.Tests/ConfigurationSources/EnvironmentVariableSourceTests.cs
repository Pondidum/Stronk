using System;
using System.Collections;
using Shouldly;
using Stronk.ConfigurationSources;
using Xunit;

namespace Stronk.Tests.ConfigurationSources
{
	public class EnvironmentVariableSourceTests
	{
		private static EnvironmentVariableSource Source(string prefix = "", Hashtable values = null)
		{
			return new EnvironmentVariableSource(prefix: prefix, source: values ?? new Hashtable
			{
				["PATH"] = "/d/dev"
			});
		}

		[Fact]
		public void When_fetching_all_available_variables()
		{
			Source().GetAvailableKeys().ShouldNotBeEmpty();
		}

		[Fact]
		public void When_getting_an_existing_key()
		{
			Source().GetValue("PATH").ShouldNotBeNullOrWhiteSpace();
		}

		[Fact]
		public void When_getting_an_existing_value_with_different_casting()
		{
			Source().GetValue("PAth").ShouldNotBeNullOrWhiteSpace();
		}

		[Fact]
		public void When_getting_a_non_existing_key()
		{
			Source().GetValue(Guid.NewGuid().ToString()).ShouldBeNull();
		}

		[Fact]
		public void When_a_prefix_is_specified_and_all_keys_are_listed()
		{
			var source = Source("wat:", new Hashtable
			{
				["Path"] = "/d/dev/projects",
				["wat:again"] = "very",
				["WAT:this"] = "yes"
			});

			source
				.GetAvailableKeys()
				.ShouldBe(new[] { "again", "this" }, ignoreOrder: true);
		}
	}
}
