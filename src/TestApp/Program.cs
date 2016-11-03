using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stronk;

namespace TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var config = new MassiveConfig();
			var configuration = new StronkConfiguration();
			var provider = new InMemoryProvider();

			config.FromAppConfig(configuration, provider);
		}
	}

	public class InMemoryProvider : IConfigurationProvider
	{
		public NameValueCollection AppSettings { get; }
		public ConnectionStringSettingsCollection ConnectionStrings { get; }

		public InMemoryProvider()
		{
			AppSettings = new NameValueCollection();
			ConnectionStrings = new ConnectionStringSettingsCollection();

			for (int i = 0; i < 200; i++)
				AppSettings["SomeProperty" + i] = i.ToString();
		}
	}
}
