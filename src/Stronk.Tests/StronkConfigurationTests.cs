using System;
using System.Linq;
using Shouldly;
using Stronk.PropertySelection;
using Stronk.ValueConversion;
using Stronk.ValueSelection;
using Xunit;

namespace Stronk.Tests
{
	public class StronkConfigurationTests
	{
		[Fact]
		public void By_default_the_collections_are_initialised()
		{
			var config = new StronkConfiguration();
			var converters = config.ValueConverters.Select(c => c.GetType());
			var propertySelectors = config.PropertySelectors.Select(c => c.GetType());
			var valueSelectors = config.ValueSelectors.Select(t => t.GetType());

			config.ShouldSatisfyAllConditions(
				() => converters.ShouldBe(new[] { typeof(LambdaValueConverter<Uri>), typeof(EnumValueConverter), typeof(FallbackValueConverter) }),
				() => propertySelectors.ShouldBe(new[] { typeof(PrivateSetterPropertySelector), typeof(BackingFieldPropertySelector) }),
				() => valueSelectors.ShouldBe(new[] { typeof(PropertyNameValueSelector) })
			);
		}

		[Fact]
		public void When_a_value_converter_is_added()
		{
			var config = new StronkConfiguration();
			config.Add(new DtoValueConverter());

			config.ValueConverters.Last().ShouldBeOfType<DtoValueConverter>();
		}

		[Fact]
		public void When_a_value_converter_is_added_before_an_existing_converter()
		{
			var config = new StronkConfiguration();
			config.AddBefore<LambdaValueConverter<Uri>>(new DtoValueConverter());

			var converters = config.ValueConverters.Select(c => c.GetType()).ToList();
			var searchIndex = converters.IndexOf(typeof(LambdaValueConverter<Uri>));
			var insertIndex = converters.IndexOf(typeof(DtoValueConverter));

			insertIndex.ShouldBe(searchIndex - 1);
		}

		[Fact]
		public void When_a_value_converter_is_added_after_an_existing_converter()
		{
			var config = new StronkConfiguration();
			config.AddAfter<LambdaValueConverter<Uri>>(new DtoValueConverter());

			var converters = config.ValueConverters.Select(c => c.GetType()).ToList();
			var searchIndex = converters.IndexOf(typeof(LambdaValueConverter<Uri>));
			var insertIndex = converters.IndexOf(typeof(DtoValueConverter));

			insertIndex.ShouldBe(searchIndex + 1);
		}

		[Fact]
		public void When_a_value_converter_is_added_before_a_non_existing_converter()
		{
			var config = new StronkConfiguration();
			Should.Throw<StronkConfigurationException>(() => config.AddBefore<LambdaValueConverter<object>>(new DtoValueConverter()));
		}

		[Fact]
		public void When_a_value_converter_is_added_after_a_non_existing_converter()
		{
			var config = new StronkConfiguration();
			Should.Throw<StronkConfigurationException>(() => config.AddAfter<LambdaValueConverter<object>>(new DtoValueConverter()));
		}

		private class Dto { }

		private class DtoValueConverter : IValueConverter
		{
			public bool CanMap(Type target) => target == typeof(Dto);
			public object Map(Type target, string value) => new Dto();
		}
	}
}