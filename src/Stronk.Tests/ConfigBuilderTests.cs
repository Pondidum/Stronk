using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Stronk.ConfigurationSourcing;
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
		private readonly StronkOptions _options;
		private readonly IDictionary<string, string> _settings;

		public ConfigBuilderTests()
		{
			_target = new TargetConfig();
			_policy = new ErrorPolicy();
			_settings = new Dictionary<string, string>();

			_options = new StronkOptions
			{
				ErrorPolicy = _policy,
				ConfigSource = new DictionaryConfigurationSource(_settings)
			};

			_builder = new ConfigBuilder(_options);
		}

		[Fact]
		public void When_a_source_value_is_not_found_and_policy_is_throw()
		{
			_policy.OnSourceValueNotFound = new SourceValueNotFoundPolicy(PolicyActions.ThrowException);

			Should
				.Throw<SourceValueNotFoundException>(() => _builder.Populate(_target))
				.Message.ShouldStartWith("Unable to find a value for 'Int32' property 'Value'");
		}

		[Fact]
		public void When_a_source_value_is_not_found_and_policy_is_skip()
		{
			_policy.OnSourceValueNotFound = new SourceValueNotFoundPolicy(PolicyActions.Skip);

			_builder.Populate(_target);

			_target.Value.ShouldBe(0);
		}

		[Fact]
		public void When_a_converter_cannot_be_found_and_policy_is_throw()
		{
			_policy.OnConverterNotFound = new ConverterNotFoundPolicy(PolicyActions.ThrowException);
			_settings["Value"] = "12";

			_options.ValueConverters = new List<IValueConverter>
			{
				new LambdaValueConverter<Uri>(val => new Uri(val)),
			};

			Should
				.Throw<Exception>(() => _builder.Populate(_target))
				.Message.ShouldStartWith("None of the following converters were suitable to handle property 'Value' of type 'Int32':");
		}

		[Fact]
		public void When_a_converter_cannot_be_found_and_policy_is_skip()
		{
			_policy.OnConverterNotFound = new ConverterNotFoundPolicy(PolicyActions.Skip);
			_settings["Value"] = "12";

			_options.ValueConverters = new List<IValueConverter>
			{
				new LambdaValueConverter<Uri>(val => new Uri(val)),
			};

			_builder.Populate(_target);

			_target.Value.ShouldBe(0);
		}

		[Fact]
		public void When_a_converter_throws_an_exception_and_policy_is_throw()
		{
			_policy.ConversionExceptionPolicy = new ConversionExceptionPolicy(ConverterExceptionPolicy.ThrowException);
			_settings["Value"] = "12";

			_options.ValueConverters = new List<IValueConverter>
			{
				new LambdaValueConverter<int>(val => { throw new NotFiniteNumberException(); }),
			};

			Should
				.Throw<ValueConversionException>(() => _builder.Populate(_target))
				.InnerException.ShouldBeOfType<NotFiniteNumberException>();
		}

		[Fact]
		public void When_a_converter_throws_an_exception_and_policy_is_skip()
		{
			_policy.ConversionExceptionPolicy = new ConversionExceptionPolicy(ConverterExceptionPolicy.Skip);
			_settings["Value"] = "12";

			_options.ValueConverters = new List<IValueConverter>
			{
				new LambdaValueConverter<int>(val => { throw new NotFiniteNumberException(); }),
			};

			_builder.Populate(_target);

			_target.Value.ShouldBe(0);
		}

		[Fact]
		public void When_a_converter_throws_and_policy_is_fallback_or_throw_and_there_is_no_fallback()
		{
			_policy.ConversionExceptionPolicy = new ConversionExceptionPolicy(ConverterExceptionPolicy.FallbackOrThrow);
			_settings["Value"] = "12";

			_options.ValueConverters = new List<IValueConverter>
			{
				new LambdaValueConverter<int>(val => { throw new NotFiniteNumberException(); }),
			};

			Should
				.Throw<ValueConversionException>(() => _builder.Populate(_target))
				.InnerException.ShouldBeOfType<NotFiniteNumberException>();
		}

		[Fact]
		public void When_a_converter_throws_and_policy_is_fallback_or_throw_and_fallback_works()
		{
			_policy.ConversionExceptionPolicy = new ConversionExceptionPolicy(ConverterExceptionPolicy.FallbackOrThrow);
			_settings["Value"] = "12";

			_options.ValueConverters = new List<IValueConverter>
			{
				new LambdaValueConverter<int>(val => { throw new NotFiniteNumberException(); }),
				new FallbackValueConverter()
			};

			_builder.Populate(_target);

			_target.Value.ShouldBe(12);
		}

		[Fact]
		public void When_a_converter_throws_and_policy_is_fallback_or_throw_and_fallback_fails()
		{
			_policy.ConversionExceptionPolicy = new ConversionExceptionPolicy(ConverterExceptionPolicy.FallbackOrThrow);
			_settings["Value"] = "12";

			_options.ValueConverters = new List<IValueConverter>
			{
				new LambdaValueConverter<int>(val => { throw new NotFiniteNumberException(); }),
				new LambdaValueConverter<int>(val => { throw new IndexOutOfRangeException(); }),
			};

			var ex = Should
				.Throw<ValueConversionException>(() => _builder.Populate(_target));

			ex.InnerExceptions.First().ShouldBeOfType<NotFiniteNumberException>();
			ex.InnerExceptions.Last().ShouldBeOfType<IndexOutOfRangeException>();
		}

		[Fact]
		public void When_a_converter_throws_and_policy_is_fallback_or_throw_and_multiple_fallbacks_fail()
		{
			_policy.ConversionExceptionPolicy = new ConversionExceptionPolicy(ConverterExceptionPolicy.FallbackOrThrow);
			_settings["Value"] = "12";

			_options.ValueConverters = new List<IValueConverter>
			{
				new LambdaValueConverter<int>(val => { throw new NotFiniteNumberException(); }),
				new LambdaValueConverter<int>(val => { throw new IndexOutOfRangeException(); }),
				new LambdaValueConverter<int>(val => { throw new NotFiniteNumberException(); }),
				new LambdaValueConverter<int>(val => { throw new IndexOutOfRangeException(); }),
			};

			var ex = Should
				.Throw<ValueConversionException>(() => _builder.Populate(_target));

			ex.InnerExceptions.Count().ShouldBe(4);
		}

		[Fact]
		public void When_a_converter_throws_and_policy_is_fallback_or_skip_and_there_is_no_fallback()
		{
			_policy.ConversionExceptionPolicy = new ConversionExceptionPolicy(ConverterExceptionPolicy.FallbackOrSkip);
			_settings["Value"] = "12";

			_options.ValueConverters = new List<IValueConverter>
			{
				new LambdaValueConverter<int>(val => { throw new NotFiniteNumberException(); }),
			};

			_builder.Populate(_target);

			_target.Value.ShouldBe(0);
		}

		[Fact]
		public void When_a_converter_throws_and_policy_is_fallback_or_skip_and_fallback_works()
		{
			_policy.ConversionExceptionPolicy = new ConversionExceptionPolicy(ConverterExceptionPolicy.FallbackOrSkip);
			_settings["Value"] = "12";

			_options.ValueConverters = new List<IValueConverter>
			{
				new LambdaValueConverter<int>(val => { throw new NotFiniteNumberException(); }),
				new FallbackValueConverter()
			};

			_builder.Populate(_target);

			_target.Value.ShouldBe(12);
		}

		[Fact]
		public void When_a_converter_throws_and_policy_is_fallback_or_skip_and_fallback_fails()
		{
			_policy.ConversionExceptionPolicy = new ConversionExceptionPolicy(ConverterExceptionPolicy.FallbackOrSkip);
			_settings["Value"] = "12";

			_options.ValueConverters = new List<IValueConverter>
			{
				new LambdaValueConverter<int>(val => { throw new NotFiniteNumberException(); }),
				new LambdaValueConverter<int>(val => { throw new IndexOutOfRangeException(); }),
			};

			_builder.Populate(_target);

			_target.Value.ShouldBe(0);
		}

		private class TargetConfig
		{
			public int Value { get; private set; }
		}
	}
}
