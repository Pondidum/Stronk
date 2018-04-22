using System.Text;
using Consul;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Stronk.Source.Consul.Tests
{
	public class ConsulConfigurationSourceTests
	{
		private readonly IConsulClient _client;
		private readonly ConsulConfigurationSource _source;

		public ConsulConfigurationSourceTests()
		{
			_client = Substitute.For<IConsulClient>();
			_source = new ConsulConfigurationSource(() => _client);
		}

		[Fact]
		public void When_getting_a_value()
		{
			_client.KV.Get("wat/is/this").Returns(new QueryResult<KVPair>
			{
				Response = new KVPair("wat/is/this") { Value = Encoding.UTF8.GetBytes("12") }
			});

			_source.GetValue("wat/is/this").ShouldBe("12");
			_client.Received().Dispose();
		}

		[Fact]
		public void When_listing_all_keys()
		{
			_client.KV.List("").Returns(new QueryResult<KVPair[]>
			{
				Response = new[]
				{
					new KVPair("one") { Value = Encoding.UTF8.GetBytes("a") },
					new KVPair("two") { Value = Encoding.UTF8.GetBytes("b") },
					new KVPair("three") { Value = Encoding.UTF8.GetBytes("c") },
				}
			});

			_source.GetAvailableKeys().ShouldBe(new[] { "one", "two", "three" });
			_client.Received().Dispose();
		}
	}
}
