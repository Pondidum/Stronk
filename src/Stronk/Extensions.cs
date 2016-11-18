namespace Stronk
{
	public static class Extensions
	{
		public static void FromAppConfig(this object target)
		{
			target.FromAppConfig(new StronkConfiguration());
		}

		public static void FromAppConfig(this object target, IStronkConfiguration configuration, IConfigurationProvider configProvider = null)
		{
			var builder = new ConfigBuilder(configuration);

			builder.Populate(target, configProvider);
		}
	}
}
