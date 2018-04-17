using System;
using System.Collections.Generic;
using Shouldly;
using Stronk.ConfigurationSources;
using Xunit;

namespace Stronk.Tests.Scenarios
{
	public class NullableProperties
	{
		[Fact]
		public void When_the_sources_dont_have_a_value()
		{
			var values = new DictionarySource(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));

			var config = new StronkConfig()
				.From.Source(values)
				.Build<NullablePropertyConfig>();

			config.SomeValue.HasValue.ShouldBeFalse();
		}

		[Fact]
		public void When_the_source_has_a_value()
		{
			var values = new DictionarySource(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
			{
				{ "SomeValue", "17" }
			});

			var config = new StronkConfig()
				.From.Source(values)
				.Build<NullablePropertyConfig>();

			config.SomeValue.HasValue.ShouldBeTrue();
			config.SomeValue.ShouldBe(17);
		}

		private class NullablePropertyConfig
		{
			public Nullable<int> SomeValue { get; private set; }
		}
	}
}
