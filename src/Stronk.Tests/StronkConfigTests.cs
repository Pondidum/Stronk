using System.Collections.Generic;
using Shouldly;
using Stronk.ConfigurationSourcing;
using Stronk.Policies;
using Xunit;

namespace Stronk.Tests
{
	public class StronkConfigTests
	{
		private IStronkConfig _config;

		[Fact]
		public void When_nothing_is_specified()
		{
			_config = new StronkConfig() as IStronkConfig;

			_config.ShouldSatisfyAllConditions(
				() => _config.ConfigSources.ShouldBe(Default.ConfigurationSources),
				() => _config.ValueSelectors.ShouldBe(Default.SourceValueSelectors),
				() => _config.ValueConverters.ShouldBe(Default.ValueConverters),
				() => _config.PropertyWriters.ShouldBe(Default.PropertyWriters),
				() => _config.ErrorPolicy.ShouldBeOfType<ErrorPolicy>()
			);
		}

		[Fact]
		public void When_you_specify_a_config_source_it_is_the_only_one_used()
		{
			var source = new DictionaryConfigurationSource(new Dictionary<string, string>());
			_config = new StronkConfig().From.Source(source) as IStronkConfig;

			_config.ConfigSources.ShouldBe(new[] { source });
		}

		[Fact]
		public void When_multiple_config_sources_are_selected()
		{
			var one = new AppConfigSource();
			var two = new DictionaryConfigurationSource(new Dictionary<string, string>());

			_config = new StronkConfig()
				.From.Source(one)
				.From.Source(two);

			_config.ConfigSources.ShouldBe(new IConfigurationSource[] { one, two });
		}
	}
}
