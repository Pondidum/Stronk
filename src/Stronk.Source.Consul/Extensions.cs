using Consul;
using Stronk.Dsl;

namespace Stronk.Source.Consul
{
	public static class Extensions
	{
		public static StronkConfig Consul(this SourceExpression self, string prefix = null, QueryOptions options = null)
		{
			return self.Source(new ConsulConfigurationSource(
				() => new ConsulClient(),
				prefix,
				options));
		}
	}
}
