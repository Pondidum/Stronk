namespace Stronk
{
	public static class Extensions
	{
		public static void FromAppConfig(this object target, IStronkConfiguration configuration = null, IConfigurationSource configSource = null)
		{
			var builder = new ConfigBuilder(configuration ?? new StronkConfiguration());

			builder.Populate(target, configSource ?? new AppConfigSource());
		}

		public static void FromWebConfig(this object target, IStronkConfiguration configuration = null, IConfigurationSource configSource = null)
		{
			var builder = new ConfigBuilder(configuration ?? new StronkConfiguration());

			builder.Populate(target, configSource ?? new AppConfigSource());
		}
	}
}
