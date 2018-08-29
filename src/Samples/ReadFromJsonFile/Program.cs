using System;
using Stronk;

namespace ReadFromJsonFile
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Reading from settings.json...");
			Console.WriteLine("");

			var config = new StronkConfig()
				.From.Source(new JsonConfigFile("settings.json"))
				.Build<Configuration>();

			Console.WriteLine($"{nameof(config.Timeout)}: {config.Timeout.TotalSeconds} seconds");
			Console.WriteLine($"{nameof(config.Callback)}: {config.Callback}");
		}
	}
}
