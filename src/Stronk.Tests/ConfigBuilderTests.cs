﻿using System;
using System.Collections.Specialized;
using System.Configuration;
using NSubstitute;
using Shouldly;
using Stronk.Policies;
using Stronk.ValueConversion;
using Xunit;

namespace Stronk.Tests
{
	public class ConfigBuilderTests
	{
		private readonly ConfigBuilder _builder;
		private readonly ErrorPolicy _policy;
		private readonly TargetConfig _target;
		private readonly IConfigurationSource _source;
		private readonly IStronkOptions _options;

		public ConfigBuilderTests()
		{
			_target = new TargetConfig();
			_policy = new ErrorPolicy();

			_options = Substitute.For<IStronkOptions>();
			_options.ErrorPolicy.Returns(_policy);
			_options.ValueSelectors.Returns(Default.SourceValueSelectors);
			_options.ValueConverters.Returns(Default.ValueConverters);
			_options.PropertySelectors.Returns(Default.PropertySelectors);

			_builder = new ConfigBuilder(_options);

			_source = Substitute.For<IConfigurationSource>();
			_source.AppSettings.Returns(new NameValueCollection());
			_source.ConnectionStrings.Returns(new ConnectionStringSettingsCollection());
		}

		[Fact]
		public void When_a_source_value_is_not_found_and_policy_is_throw()
		{
			_policy.OnSourceValueNotFound = PolicyActions.ThrowException;

			Should
				.Throw<SourceValueNotFoundException>(() => _builder.Populate(_target, _source))
				.Message.ShouldStartWith("Unable to find a value for property 'Value' using the following selectors:");
		}

		[Fact]
		public void When_a_source_value_is_not_found_and_policy_is_skip()
		{
			_policy.OnSourceValueNotFound = PolicyActions.Skip;

			_builder.Populate(_target, _source);

			_target.Value.ShouldBe(0);
		}

		[Fact]
		public void When_a_converter_cannot_be_found_and_policy_is_throw()
		{
			_policy.OnNoConverterFound = PolicyActions.ThrowException;
			_source.AppSettings["Value"] = "12";

			_options.ValueConverters.Returns(new[]
			{
				new LambdaValueConverter<Uri>(val => new Uri(val)),
			});

			Should
				.Throw<Exception>(() => _builder.Populate(_target, _source))
				.Message.ShouldStartWith("None of the following converters was suitable to handle property 'Value' of type 'Int32':");
		}

		[Fact]
		public void When_a_converter_cannot_be_found_and_policy_is_skip()
		{
			_policy.OnNoConverterFound = PolicyActions.Skip;
			_source.AppSettings["Value"] = "12";

			_options.ValueConverters.Returns(new[]
			{
				new LambdaValueConverter<Uri>(val => new Uri(val)),
			});

			_builder.Populate(_target, _source);

			_target.Value.ShouldBe(0);
		}

		private class TargetConfig
		{
			public int Value { get; private set; }
		}
	}
}
