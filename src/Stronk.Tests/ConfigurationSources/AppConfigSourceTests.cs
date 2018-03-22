using Shouldly;
using Stronk.ConfigurationSources;
using Xunit;

namespace Stronk.Tests.ConfigurationSources
{
	public class AppConfigSourceTests
	{
		private readonly AppConfigSource _source;

		public AppConfigSourceTests()
		{
			_source = new AppConfigSource();
		}

		[Fact]
		public void Can_read_an_appsetting_by_name()
		{
			_source.GetValue("Environment").ShouldBe("test");
		}

		[Fact]
		public void Can_read_an_appsetting_by_case_insensitive_name()
		{
			_source.GetValue("EnviRONment").ShouldBe("test");
		}

		[Fact]
		public void Can_read_an_connection_string_by_name()
		{
			_source.GetValue("DefaultDB").ShouldBe("something here");
		}

		[Fact]
		public void Can_read_an_connection_string_by_case_insensitive_name()
		{
			_source.GetValue("deFAULTdb").ShouldBe("something here");
		}
	}
}
