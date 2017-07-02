using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Stronk;
using Stronk.ConfigurationSourcing;

namespace TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.WriteTo.ColoredConsole()
				.CreateLogger();

			var config = new SmallConfig();
			var options = new StronkOptions
			{
				Logger = message => Log.Information(message.Template, message.Args)
			};

			var provider = new InMemorySource();

			config.FromAppConfig(options, provider);

			Console.WriteLine("Done...");
			Console.ReadKey();
		}
	}

	public class InMemorySource : IConfigurationSource
	{
		public IDictionary<string, string> AppSettings { get; }
		public IDictionary<string, ConnectionStringSettings> ConnectionStrings { get; }

		public InMemorySource()
		{
			AppSettings = new Dictionary<string, string>();
			ConnectionStrings = new Dictionary<string, ConnectionStringSettings>();

			for (int i = 0; i < 200; i++)
				AppSettings["SomeProperty" + i] = i.ToString();
		}
	}
}
