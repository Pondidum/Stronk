using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Stronk.ConfigurationSources;
using Stronk.Policies;
using Stronk.PropertyMappers;
using Stronk.PropertyWriters;
using Stronk.ValueConverters;
using Xunit;

namespace Stronk.Tests
{
	public class StronkConfigTests
	{
		private IStronkConfig _config;

		[Fact]
		public void When_nothing_is_specified()
		{
			_config = new StronkConfig();

			_config.ShouldSatisfyAllConditions(
				() => _config.ConfigSources.ShouldBe(Default.ConfigurationSources),
				() => _config.Mappers.ShouldBe(Default.SourceValueSelectors),
				() => _config.ValueConverters.ShouldBe(Default.ValueConverters),
				() => _config.PropertyWriters.ShouldBe(Default.PropertyWriters),
				() => _config.ErrorPolicy.ShouldBeOfType<ErrorPolicy>()
			);
		}

		[Fact]
		public void When_you_specify_a_config_source_it_is_the_only_one_used()
		{
			var source = new DictionarySource(new Dictionary<string, string>());
			_config = new StronkConfig().From.Source(source);

			_config.ConfigSources.ShouldBe(new[] { source });
		}

		[Fact]
		public void When_multiple_config_sources_are_selected()
		{
			var one = new AppConfigSource();
			var two = new DictionarySource(new Dictionary<string, string>());

			_config = new StronkConfig()
				.From.Source(one)
				.From.Source(two);

			_config.ConfigSources.ShouldBe(new IConfigurationSource[] { one, two });
		}

		[Fact]
		public void When_one_property_writer_is_specified()
		{
			var setters = new PrivateSetterPropertyWriter();
			_config = new StronkConfig().Write.To(setters);

			_config.PropertyWriters.ShouldBe(new[] { setters });
		}

		[Fact]
		public void When_multiple_property_writers_are_specified()
		{
			var one = new PrivateSetterPropertyWriter();
			var two = new BackingFieldPropertyWriter();

			_config = new StronkConfig()
				.Write.To(one)
				.Write.To(two);

			_config.PropertyWriters.ShouldBe(new IPropertyWriter[] { one, two });
		}

		[Fact]
		public void When_one_value_selector_is_specified()
		{
			var one = new PropertyNamePropertyMapper();
			_config = new StronkConfig().Map.With(one);

			_config.Mappers.ShouldBe(new[] { one });
		}

		[Fact]
		public void When_two_value_selectors_are_specified()
		{
			var one = new PropertyNamePropertyMapper();
			var two = new PropertyNamePropertyMapper();

			_config = new StronkConfig()
				.Map.With(one)
				.Map.With(two);

			_config.Mappers.ShouldBe(new[] { one, two });
		}


		[Fact]
		public void When_one_value_converter_only_is_specified()
		{
			var one = new EnumValueConverter();

			_config = new StronkConfig()
				.Convert.UsingOnly(one);

			_config.ValueConverters.ShouldBe(new IValueConverter[] { one });
		}

		[Fact]
		public void When_one_value_converter_is_specified()
		{
			var one = new EnumValueConverter();

			_config = new StronkConfig()
				.Convert.Using(one);

			_config.ValueConverters.ShouldBe(new IValueConverter[] { one }.Concat(Default.ValueConverters));
		}

		[Fact]
		public void When_two_value_converters_are_specified()
		{
			var one = new EnumValueConverter();
			var two = new CsvValueConverter();

			_config = new StronkConfig()
				.Convert.Using(one, two);

			_config.ValueConverters.ShouldBe(new IValueConverter[] { one, two }.Concat(Default.ValueConverters));
		}

		[Fact]
		public void When_two_value_converters_are_specified_and_one_only_specified()
		{
			var one = new EnumValueConverter();
			var two = new CsvValueConverter();
			var three = new FallbackValueConverter();

			_config = new StronkConfig()
				.Convert.Using(one, two)
				.Convert.UsingOnly(three);

			_config.ValueConverters.ShouldBe(new IValueConverter[] { one, two, three });
		}

		[Fact]
		public void When_specifying_the_error_policy()
		{
			var policy = new ErrorPolicy();

			_config = new StronkConfig()
				.HandleErrors.Using(policy);

			_config.ErrorPolicy.ShouldBe(policy);
		}

		[Fact]
		public void When_specifying_one_logger()
		{
			LogMessage message = null;

			_config = new StronkConfig()
				.Log.Using(m => message = m);

			_config.WriteLog("wat", 5, "values", "appear", "after", "it");

			message.ShouldSatisfyAllConditions(
				() => message.Template.ShouldBe("wat"),
				() => message.Args.ShouldBe(new object[] { 5, "values", "appear", "after", "it" })
			);
		}

		[Fact]
		public void When_two_loggers_are_specified()
		{
			LogMessage one = null;
			LogMessage two = null;

			_config = new StronkConfig()
				.Log.Using(message => one = message)
				.Log.Using(message => two = message);

			_config.WriteLog("test", "value");

			_config.ShouldSatisfyAllConditions(
				() => one.Template.ShouldBe("test"),
				() => one.Args.ShouldBe(new[] { "value" }),
				() => two.Template.ShouldBe("test"),
				() => two.Args.ShouldBe(new[] { "value" })
			);
		}
	}
}
