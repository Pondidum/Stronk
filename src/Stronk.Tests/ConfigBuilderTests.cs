﻿using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Stronk.ConfigurationSources;
using Stronk.Policies;
using Stronk.ValueConverters;
using Xunit;

namespace Stronk.Tests
{
	public class ConfigBuilderTests
	{
		private readonly ConfigBuilder _builder;
		private readonly TargetConfig _target;
		private readonly StronkConfig _options;
		private readonly IDictionary<string, string> _settings;

		public ConfigBuilderTests()
		{
			_target = new TargetConfig();
			_settings = new Dictionary<string, string>();

			_options = new StronkConfig()
				.From.Source(new DictionarySource(_settings));

			_builder = new ConfigBuilder(_options);
		}

		[Fact]
		public void When_a_source_value_is_not_found_and_policy_is_throw()
		{

			Should
				.Throw<SourceValueNotFoundException>(() => _builder.Populate(_target))
				.Message.ShouldStartWith("Unable to find a value for 'Int32' property 'Value'");
		}

		[Fact]
		public void When_a_converter_cannot_be_found_and_policy_is_throw()
		{
			_settings["Value"] = "12";

			_options.Convert.UsingOnly(new LambdaValueConverter<Uri>(val => new Uri(val)));

			Should
				.Throw<Exception>(() => _builder.Populate(_target))
				.Message.ShouldStartWith("None of the following converters were suitable to handle property 'Value' of type 'Int32':");
		}

		[Fact]
		public void When_a_converter_throws_an_exception_and_policy_is_throw()
		{
			_settings["Value"] = "12";

			_options.Convert.UsingOnly(new LambdaValueConverter<int>(val => { throw new NotFiniteNumberException(); }));

			Should
				.Throw<ValueConversionException>(() => _builder.Populate(_target))
				.InnerException.ShouldBeOfType<NotFiniteNumberException>();
		}

		[Fact]
		public void When_a_converter_throws_and_policy_is_fallback_or_throw_and_there_is_no_fallback()
		{
			_settings["Value"] = "12";

			_options.Convert.UsingOnly(new LambdaValueConverter<int>(val => { throw new NotFiniteNumberException(); }));

			Should
				.Throw<ValueConversionException>(() => _builder.Populate(_target))
				.InnerException.ShouldBeOfType<NotFiniteNumberException>();
		}

		[Fact]
		public void When_a_converter_throws_and_policy_is_fallback_or_throw_and_fallback_works()
		{
			_settings["Value"] = "12";

			_options.Convert.UsingOnly(
				new LambdaValueConverter<int>(val => { throw new NotFiniteNumberException(); }),
				new FallbackValueConverter());

			_builder.Populate(_target);

			_target.Value.ShouldBe(12);
		}

		[Fact]
		public void When_a_converter_throws_and_policy_is_fallback_or_throw_and_fallback_fails()
		{
			_settings["Value"] = "12";

			_options.Convert.UsingOnly(new LambdaValueConverter<int>(val => { throw new NotFiniteNumberException(); }),
				new LambdaValueConverter<int>(val => { throw new IndexOutOfRangeException(); }));

			var ex = Should
				.Throw<ValueConversionException>(() => _builder.Populate(_target));

			ex.InnerExceptions.First().ShouldBeOfType<NotFiniteNumberException>();
			ex.InnerExceptions.Last().ShouldBeOfType<IndexOutOfRangeException>();
		}

		[Fact]
		public void When_a_converter_throws_and_policy_is_fallback_or_throw_and_multiple_fallbacks_fail()
		{
			_settings["Value"] = "12";

			_options.Convert.UsingOnly(new LambdaValueConverter<int>(val => { throw new NotFiniteNumberException(); }),
				new LambdaValueConverter<int>(val => { throw new IndexOutOfRangeException(); }),
				new LambdaValueConverter<int>(val => { throw new NotFiniteNumberException(); }),
				new LambdaValueConverter<int>(val => { throw new IndexOutOfRangeException(); }));

			var ex = Should
				.Throw<ValueConversionException>(() => _builder.Populate(_target));

			ex.InnerExceptions.Count().ShouldBe(4);
		}

		[Fact]
		public void When_a_converter_throws_and_policy_is_fallback_or_skip_and_fallback_works()
		{
			_settings["Value"] = "12";

			_options.Convert.UsingOnly(
				new LambdaValueConverter<int>(val => { throw new NotFiniteNumberException(); }),
				new FallbackValueConverter());

			_builder.Populate(_target);

			_target.Value.ShouldBe(12);
		}

		private class TargetConfig
		{
			public int Value { get; private set; }
		}
	}
}
