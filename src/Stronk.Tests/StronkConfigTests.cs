﻿using System.Collections.Generic;
using Shouldly;
using Stronk.ConfigurationSourcing;
using Stronk.Policies;
using Stronk.PropertyWriters;
using Stronk.SourceValueSelection;
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

		[Fact]
		public void When_one_property_writer_is_specified()
		{
			var setters = new PrivateSetterPropertyWriter();
			_config = new StronkConfig().Write.To(setters);

			_config.PropertyWriters.ShouldBe(new[] { setters });
		}

		[Fact]
		public void When_multiple_property_writers_are_specified()
		{
			var one = new PrivateSetterPropertyWriter();
			var two = new BackingFieldPropertyWriter();

			_config = new StronkConfig()
				.Write.To(one)
				.Write.To(two);

			_config.PropertyWriters.ShouldBe(new IPropertyWriter[] { one, two });
		}

		[Fact]
		public void When_one_value_selector_is_specified()
		{
			var one = new PropertyNameSourceValueSelector();
			_config = new StronkConfig().Map.With(one);

			_config.ValueSelectors.ShouldBe(new[] { one });
		}

		[Fact]
		public void When_two_value_selectors_are_specified()
		{
			var one = new PropertyNameSourceValueSelector();
			var two = new PropertyNameSourceValueSelector();

			_config = new StronkConfig()
				.Map.With(one)
				.Map.With(two);

			_config.ValueSelectors.ShouldBe(new[] { one, two });
		}
	}
}
