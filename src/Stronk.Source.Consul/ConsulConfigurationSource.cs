using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using Stronk.ConfigurationSources;

namespace Stronk.Source.Consul
{
	public class ConsulConfigurationSource : IConfigurationSource
	{
		private readonly Func<IConsulClient> _clientFactory;
		private readonly QueryOptions _options;
		private readonly string _prefix;

		public ConsulConfigurationSource(Func<IConsulClient> clientFactory, string prefix = null, QueryOptions options = null)
		{
			_clientFactory = clientFactory;
			_options = options ?? QueryOptions.Default;
			_prefix = WithTrailingSlash(prefix ?? string.Empty);
		}

		public string GetValue(string key) => Query(
			client => client.Get(_prefix + key, _options),
			result => result != null ? AsString(result.Value) : null);

		public IEnumerable<string> GetAvailableKeys() => Query(
			client => client.List(_prefix, _options),
			result => result
				.Select(TheKey)
				.Select(WithoutPrefix)
				.ToArray());

		private TReturn Query<TResponse, TReturn>(Func<IKVEndpoint, Task<QueryResult<TResponse>>> query, Func<TResponse, TReturn> transform)
		{
			using (var client = _clientFactory())
				return transform(query(client.KV).Result.Response);
		}

		private static string WithTrailingSlash(string input) =>
			input.EndsWith("/")
				? input
				: input + "/";

		private static string AsString(byte[] bytes) =>
			Encoding.UTF8.GetString(bytes, 0, bytes.Length);

		private static string TheKey(KVPair pair) => pair.Key;
		private string WithoutPrefix(string key) => key.StartsWith(_prefix)
			? key.Substring(_prefix.Length)
			: key;
	}
}
