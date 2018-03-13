using Shouldly;
using Stronk.Policies;
using Xunit;

namespace Stronk.Tests
{
	public class StronkConfigTests
	{
		[Fact]
		public void When_nothing_is_specified()
		{
			var config = new StronkConfig() as IStronkConfig;

			config.ShouldSatisfyAllConditions(
				() => config.ConfigSources.ShouldBe(Default.ConfigurationSources),
				() => config.ValueSelectors.ShouldBe(Default.SourceValueSelectors),
				() => config.ValueConverters.ShouldBe(Default.ValueConverters),
				() => config.PropertyWriters.ShouldBe(Default.PropertyWriters),
				() => config.ErrorPolicy.ShouldBeOfType<ErrorPolicy>()
			);
		}
	}
}
