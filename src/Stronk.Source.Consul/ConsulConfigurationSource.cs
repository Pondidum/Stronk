using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Consul;
using Stronk.ConfigurationSources;

namespace Stronk.Source.Consul
{
	public class ConsulConfigurationSource : IConfigurationSource
	{
		private readonly Func<IConsulClient> _clientFactory;

		public ConsulConfigurationSource(Func<IConsulClient> clientFactory)
		{
			_clientFactory = clientFactory;
		}

		public string GetValue(string key)
		{
			using (var client = _clientFactory())
			{
				var response = client.KV.Get(key).Result.Response;

				return response != null
					? AsString(response.Value)
					: null;
			}
		}

		public IEnumerable<string> GetAvailableKeys()
		{
			using (var client = _clientFactory())
			{
				var response = client.KV.List("").Result.Response;

				return response.Select(r => r.Key).ToArray();
			}
		}

		private static string AsString(byte[] bytes) => Encoding.UTF8.GetString(bytes, 0, bytes.Length);
	}
}
