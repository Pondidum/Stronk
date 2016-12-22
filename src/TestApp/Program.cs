using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Stronk;

namespace TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.WriteTo.ColoredConsole()
				.CreateLogger();

			var config = new MassiveConfig();
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
		public NameValueCollection AppSettings { get; }
		public ConnectionStringSettingsCollection ConnectionStrings { get; }

		public InMemorySource()
		{
			AppSettings = new NameValueCollection();
			ConnectionStrings = new ConnectionStringSettingsCollection();

			for (int i = 0; i < 200; i++)
				AppSettings["SomeProperty" + i] = i.ToString();
		}
	}
}
