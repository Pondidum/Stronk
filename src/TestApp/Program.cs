using System;
using System.Collections.Generic;
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
			var provider = new AppConfigProvider();

			config.FromAppConfig(configuration, provider);
		}
	}
}
