using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Stronk;
using Stronk.Source.Consul;

namespace ReadFromConsul
{
	internal class Program
	{
		public const string ApplicationName = "ReadFromConsul";

		public static async Task<int> Main(string[] args)
		{
			using (var consul = await LaunchConsul())
			{
				var config = new StronkConfig()
					.From.Consul(prefix: ApplicationName)
					.Build<Configuration>();

				Console.WriteLine("Values read from Consul:");
				Console.WriteLine($"* {nameof(config.Timeout)}: {config.Timeout.TotalSeconds} seconds");
				Console.WriteLine($"* {nameof(config.Callback)}: {config.Callback}");

				consul.Close();
			}

			return 0;
		}

		private static async Task<Process> LaunchConsul()
		{
			var process = Process.Start("consul.exe", "agent -dev");

			Console.WriteLine("Consul running...");

			using (var http = new HttpClient())
			{
				await http.PutAsync($"http://localhost:8500/v1/kv/{ApplicationName}/Timeout", new StringContent("00:00:25"));
				await http.PutAsync($"http://localhost:8500/v1/kv/{ApplicationName}/Callback", new StringContent("https://localhost/test"));
			}

			Console.WriteLine("Test values written to consul");

			return process;
		}
	}
}
