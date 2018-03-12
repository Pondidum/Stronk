using Stronk.Dsl;

namespace Stronk.ConfigurationSourcing
{
	public static class Extensions
	{
		public static StronkConfig AppSettings(this ISourceExpression self)
		{
			return self.Source(new AppConfigSource());
		}
	}
}
