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
			var configuration = new StronkOptions();
			var provider = new InMemorySource();

			config.FromAppConfig(configuration, provider);
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
