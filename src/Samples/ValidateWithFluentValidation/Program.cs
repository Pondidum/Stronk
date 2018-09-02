using System;
using Stronk;
using Stronk.Validation.FluentValidation;

namespace ValidateWithFluentValidation
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				Console.WriteLine("Try changing the app.config values to be invalid:");
				Console.WriteLine("* Timeout should be between 1 and 120 seconds");
				Console.WriteLine("* Callback should be HTTPS, and the host should be either 'localhost' or 'internal'");
				Console.WriteLine("");

				var config = new StronkConfig()
					.Validate.With<ConfigurationValidator>()
					.Build<Configuration>();

				Console.WriteLine($"{nameof(config.Timeout)}: {config.Timeout.TotalSeconds} seconds");
				Console.WriteLine($"{nameof(config.Callback)}: {config.Callback}");
			}
			catch (Exception e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine(e.ToString());
				Console.ResetColor();
			}
		}
	}
}
