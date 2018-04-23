using System;
using System.Text;
using System.Threading.Tasks;
using Consul;
using Shouldly;

namespace Stronk.Source.Consul.Tests
{
	public class IntegrationTests : IDisposable
	{
		private ConsulConfigurationSource _source;
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

			await Write(key, value);

			_source.GetValue(Prefixed(key)).ShouldBe(value);
		}

		[RequiresConsulFact]
		public async Task When_listing_keys()
		{
			await Write("a", "one");
			await Write("b", "two");
			await Write("c", "three");

			_source
				.GetAvailableKeys()
				.ShouldBe(new[] { Prefixed("a"), Prefixed("b"), Prefixed("c") }, ignoreOrder: true);
		}

		[RequiresConsulFact]
		public async Task When_using_a_prefix_listing_all_keys_doesnt_include_the_prefix()
		{
			_source = new ConsulConfigurationSource(() => new ConsulClient(), prefix: _prefix);

			await Write("a", "one");
			await Write("b", "two");
			await Write("c", "three");

			_source
				.GetAvailableKeys()
				.ShouldBe(new[] { "a", "b", "c" }, ignoreOrder: true);
		}

		[RequiresConsulFact]
		public async Task When_using_a_prefix_the_correct_value_is_returned()
		{
			_source = new ConsulConfigurationSource(() => new ConsulClient(), prefix: _prefix + "/correct");

			await Write("correct/a", "correct");
			await Write("incorrect/a", "incorrect");

			_source.GetValue("a").ShouldBe("correct");
		}

		private string Prefixed(string key) => _prefix + "/" + key;

		private Task Write(string key, string value) => _client.KV.Put(Pair(key, value));

		private KVPair Pair(string key, string value) => new KVPair(Prefixed(key))
		{
			Value = Encoding.UTF8.GetBytes(value)
		};

		public void Dispose()
		{
			_client.KV.DeleteTree(_prefix).Wait();
			_client.Dispose();
		}
	}
}
