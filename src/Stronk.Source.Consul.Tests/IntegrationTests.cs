using System;
using System.Text;
using System.Threading.Tasks;
using Consul;
using Shouldly;
using Xunit;

namespace Stronk.Source.Consul.Tests
{
	public class IntegrationTests : IDisposable
	{
		private readonly ConsulConfigurationSource _source;
		private readonly ConsulClient _client;
		private readonly string _prefix;

		public IntegrationTests()
		{
			_client = new ConsulClient();
			_source = new ConsulConfigurationSource(() => new ConsulClient());
			_prefix = Guid.NewGuid().ToString();
		}

		[RequiresConsulFact]
		public void When_the_key_doesnt_exist()
		{
			_source.GetValue(Guid.NewGuid().ToString()).ShouldBe(null);
		}

		[RequiresConsulFact]
		public async Task When_the_key_exists()
		{
			var key = Guid.NewGuid().ToString();
			var value = Guid.NewGuid().ToString();

			await _client.KV.Put(new KVPair(_prefix + "/" + key) { Value = Encoding.UTF8.GetBytes(value) });

			_source.GetValue(_prefix + "/" + key).ShouldBe(value);
		}

		[RequiresConsulFact]
		public async Task When_listing_keys()
		{
			await _client.KV.Put(new KVPair(_prefix + "/a") { Value = Encoding.UTF8.GetBytes("one") });
			await _client.KV.Put(new KVPair(_prefix + "/b") { Value = Encoding.UTF8.GetBytes("two") });
			await _client.KV.Put(new KVPair(_prefix + "/c") { Value = Encoding.UTF8.GetBytes("three") });

			_source.GetAvailableKeys().ShouldBe(new[]
			{
				_prefix + "/a",
				_prefix + "/b",
				_prefix + "/c"
			}, ignoreOrder: true);
		}

		public void Dispose()
		{
			_client.KV.DeleteTree(_prefix).Wait();
			_client.Dispose();
		}
	}
}
