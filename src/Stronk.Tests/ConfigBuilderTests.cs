using System;
using System.Collections.Specialized;
using System.Configuration;
using NSubstitute;
using Shouldly;
using Stronk.Policies;
using Xunit;

namespace Stronk.Tests
{
	public class ConfigBuilderTests
	{
		private readonly ConfigBuilder _builder;
		private readonly ErrorPolicy _policy;
		private readonly TargetConfig _target;
		private readonly IConfigurationSource _source;

		public ConfigBuilderTests()
		{
			_target = new TargetConfig();
			_policy = new ErrorPolicy();

			_builder = new ConfigBuilder(new StronkOptions
			{
				ErrorPolicy = _policy
			});

			_source = Substitute.For<IConfigurationSource>();
			_source.AppSettings.Returns(new NameValueCollection());
			_source.ConnectionStrings.Returns(new ConnectionStringSettingsCollection());
		}

		[Fact]
		public void When_a_source_value_is_not_found_and_policy_is_throw()
		{
			_policy.OnSourceValueNotFound = PolicyActions.ThrowException;

			Should
				.Throw<Exception>(() => _builder.Populate(_target, _source))
				.Message.ShouldStartWith("Unable to find a value for property 'Value' using the following selectors:");
		}

		[Fact]
		public void When_a_source_value_is_not_found_and_policy_is_skip()
		{
			_policy.OnSourceValueNotFound = PolicyActions.Skip;

			_builder.Populate(_target, _source);

			_target.Value.ShouldBe(0);
		}

		private class TargetConfig
		{
			public int Value { get; private set; }
		}
	}
}
