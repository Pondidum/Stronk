using NSubstitute;
using Shouldly;
using Stronk.ConfigurationSourcing;
using Stronk.PropertySelection;
using Xunit;

namespace Stronk.Tests.PropertySelection
{
	public class AutoSetOnlyPropertySelectorTests
	{

		[Fact(Skip = "Does not appear to be possible for the time being.")]
		public void When_loading_values()
		{
			var source = Substitute.For<IConfigurationSource>();
			source.GetValue("Name").Returns("Testing");
			source.GetValue("Value").Returns("16");

			var selector = new AutoSetOnlyPropertySelector();

			var config = new TargetConfig(selector, source);

			config.ShouldSatisfyAllConditions(
				() => config.Name.ShouldBe("Testing"),
				() => config.Value.ShouldBe(16)
			);
		}


		private class TargetConfig
		{
			public string Name { get; }
			public int Value { get; }

			public TargetConfig(IPropertySelector selector, IConfigurationSource source)
			{
				this.FromAppConfig(
					options: new StronkOptions
					{
						PropertySelectors = new [] { selector }
					},
					configSource: source
				);
			}
		}
	}
}
