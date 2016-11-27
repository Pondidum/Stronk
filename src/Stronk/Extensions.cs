namespace Stronk
{
	public static class Extensions
	{
		public static void FromAppConfig(this object target)
		{
			target.FromAppConfig(new StronkConfiguration());
		}

		public static void FromAppConfig(this object target, IStronkConfiguration configuration, IConfigurationSource configSource = null)
		{
			var builder = new ConfigBuilder(configuration);

			builder.Populate(target, configSource);
		}

		public static void FromWebConfig(this object target)
		{
			target.FromWebConfig(new StronkConfiguration());
		}

		public static void FromWebConfig(this object target, IStronkConfiguration configuration, IConfigurationSource configSource = null)
		{
			var builder = new ConfigBuilder(configuration);

			builder.Populate(target, configSource);
		}
	}
}
