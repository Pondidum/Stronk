using Stronk.Dsl;

namespace Stronk.ConfigurationSourcing
{
	public static class Extensions
	{
		public static StronkConfig AppSettings(this SourceExpression self)
		{
			return self.Source(new AppConfigSource());
		}
	}
}
