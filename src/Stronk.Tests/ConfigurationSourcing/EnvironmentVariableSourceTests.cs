using System;
using Shouldly;
using Stronk.ConfigurationSourcing;
using Xunit;

namespace Stronk.Tests.ConfigurationSourcing
{
	public class EnvironmentVariableSourceTests
	{
		private readonly EnvironmentVariableSource _source;

		public EnvironmentVariableSourceTests()
		{
			_source = new EnvironmentVariableSource();
		}

		[Fact]
		public void When_fetching_all_available_variables()
		{
			_source.GetAvailableKeys().ShouldNotBeEmpty();
		}

		[Fact]
		public void When_getting_an_existing_key()
		{
			_source.GetValue("PATH").ShouldNotBeNullOrWhiteSpace();
		}

		[Fact]
		public void When_getting_an_existing_value_with_different_casting()
		{
			_source.GetValue("PAth").ShouldNotBeNullOrWhiteSpace();
		}

		[Fact]
		public void When_getting_a_non_existing_key()
		{
			_source.GetValue(Guid.NewGuid().ToString()).ShouldBeNull();
		}
	}
}
