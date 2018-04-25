using Shouldly;
using Xunit;

namespace Stronk.Source.Consul.Tests
{
	public class ExtensionsTests
	{
		[Fact]
		public void Consul_can_be_added_via_the_dsl()
		{
			var config = new StronkConfig().From.Consul() as IStronkConfig;

			config.ConfigSources
				.ShouldHaveSingleItem()
				.ShouldBeOfType<ConsulConfigurationSource>();
		}
	}
}
