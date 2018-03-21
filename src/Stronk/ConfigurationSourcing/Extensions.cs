using Stronk.Dsl;

namespace Stronk.ConfigurationSourcing
{
	public static class Extensions
	{
		public static StronkConfig AppSettings(this SourceExpression self)
		{
			return self.Source(new AppConfigSource());
		}

		public static StronkConfig EnvironmentVariables(this SourceExpression self, string prefix = null)
		{
			return self.Source(new EnvironmentVariableSource(prefix));
		}
	}
}
