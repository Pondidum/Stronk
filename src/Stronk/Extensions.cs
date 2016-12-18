namespace Stronk
{
	public static class Extensions
	{
		public static void FromAppConfig(this object target, StronkOptions options = null, IConfigurationSource configSource = null)
		{
			var builder = new ConfigBuilder(options ?? new StronkOptions());

			builder.Populate(target, configSource ?? new AppConfigSource());
		}

		public static void FromWebConfig(this object target, StronkOptions options = null, IConfigurationSource configSource = null)
		{
			var builder = new ConfigBuilder(options ?? new StronkOptions());

			builder.Populate(target, configSource ?? new AppConfigSource());
		}
	}
}
