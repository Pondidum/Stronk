using System;
using System.Collections.Generic;
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
		private readonly StronkConfiguration _config;

		public StronkConfigurationTests()
		{
			_config = new StronkConfiguration();
		}

		[Fact]
		public void By_default_the_collections_are_initialised()
		{
			var converters = _config.ValueConverters.Select(c => c.GetType());
			var propertySelectors = _config.PropertySelectors.Select(c => c.GetType());
			var valueSelectors = _config.ValueSelectors.Select(t => t.GetType());

			_config.ShouldSatisfyAllConditions(
				() => converters.ShouldBe(new[] { typeof(LambdaValueConverter<Uri>), typeof(EnumValueConverter), typeof(FallbackValueConverter) }),
				() => propertySelectors.ShouldBe(new[] { typeof(PrivateSetterPropertySelector), typeof(BackingFieldPropertySelector) }),
				() => valueSelectors.ShouldBe(new[] { typeof(PropertyNameValueSelector) })
			);
		}

		[Fact]
		public void When_a_value_converter_is_added()
		{
			_config.Add(new DtoValueConverter());

			_config.ValueConverters.Last().ShouldBeOfType<DtoValueConverter>();
		}

		[Fact]
		public void When_a_value_converter_is_added_before_an_existing_converter()
		{
			_config.AddBefore<LambdaValueConverter<Uri>>(new DtoValueConverter());

			InsertIndexShouldBeBefore(_config.ValueConverters, typeof(LambdaValueConverter<Uri>), typeof(DtoValueConverter));
		}

		[Fact]
		public void When_a_value_converter_is_added_after_an_existing_converter()
		{
			_config.AddAfter<LambdaValueConverter<Uri>>(new DtoValueConverter());

			InsertIndexShouldBeAfter(_config.ValueConverters, typeof(LambdaValueConverter<Uri>), typeof(DtoValueConverter));
		}

		[Fact]
		public void When_a_value_converter_is_added_before_a_non_existing_converter()
		{
			Should.Throw<StronkConfigurationException>(() => _config.AddBefore<LambdaValueConverter<object>>(new DtoValueConverter()));
		}

		[Fact]
		public void When_a_value_converter_is_added_after_a_non_existing_converter()
		{
			Should.Throw<StronkConfigurationException>(() => _config.AddAfter<LambdaValueConverter<object>>(new DtoValueConverter()));
		}




		[Fact]
		public void When_a_value_selector_is_added()
		{
			_config.Add(new DtoValueSelector());

			_config.ValueSelectors.Last().ShouldBeOfType<DtoValueSelector>();
		}

		[Fact]
		public void When_a_value_selector_is_added_before_an_existing_converter()
		{
			_config.AddBefore<PropertyNameValueSelector>(new DtoValueSelector());

			InsertIndexShouldBeBefore(_config.ValueSelectors, typeof(PropertyNameValueSelector), typeof(DtoValueSelector));
		}

		[Fact]
		public void When_a_value_selector_is_added_after_an_existing_converter()
		{
			_config.AddAfter<PropertyNameValueSelector>(new DtoValueSelector());

			InsertIndexShouldBeAfter(_config.ValueSelectors, typeof(PropertyNameValueSelector), typeof(DtoValueSelector));
		}

		[Fact]
		public void When_a_value_selector_is_added_before_a_non_existing_converter()
		{
			Should.Throw<StronkConfigurationException>(() => _config.AddBefore<UnusedValueSelector>(new DtoValueSelector()));
		}

		[Fact]
		public void When_a_value_selector_is_added_after_a_non_existing_converter()
		{
			Should.Throw<StronkConfigurationException>(() => _config.AddAfter<UnusedValueSelector>(new DtoValueSelector()));
		}



		[Fact]
		public void When_a_property_selector_is_added()
		{
			_config.Add(new DtoPropertySelector());

			_config.PropertySelectors.Last().ShouldBeOfType<DtoPropertySelector>();
		}

		[Fact]
		public void When_a_property_selector_is_added_before_an_existing_converter()
		{
			_config.AddBefore<PrivateSetterPropertySelector>(new DtoPropertySelector());

			InsertIndexShouldBeBefore(_config.PropertySelectors, typeof(PrivateSetterPropertySelector), typeof(DtoPropertySelector));
		}

		[Fact]
		public void When_a_property_selector_is_added_after_an_existing_converter()
		{
			_config.AddAfter<PrivateSetterPropertySelector>(new DtoPropertySelector());

			InsertIndexShouldBeAfter(_config.PropertySelectors, typeof(PrivateSetterPropertySelector), typeof(DtoPropertySelector));
		}

		[Fact]
		public void When_a_property_selector_is_added_before_a_non_existing_converter()
		{
			Should.Throw<StronkConfigurationException>(() => _config.AddBefore<UnusedPropertySelector>(new DtoPropertySelector()));
		}

		[Fact]
		public void When_a_property_selector_is_added_after_a_non_existing_converter()
		{
			Should.Throw<StronkConfigurationException>(() => _config.AddAfter<UnusedPropertySelector>(new DtoPropertySelector()));
		}



		private static void InsertIndexShouldBeBefore<T>(IEnumerable<T> collection, Type search, Type inserted)
		{
			var converters = collection.Select(c => c.GetType()).ToList();
			var searchIndex = converters.IndexOf(search);
			var insertIndex = converters.IndexOf(inserted);

			insertIndex.ShouldBe(searchIndex - 1);
		}

		private static void InsertIndexShouldBeAfter<T>(IEnumerable<T> collection, Type search, Type inserted)
		{
			var converters = collection.Select(c => c.GetType()).ToList();
			var searchIndex = converters.IndexOf(search);
			var insertIndex = converters.IndexOf(inserted);

			insertIndex.ShouldBe(searchIndex + 1);
		}

		private class Dto { }

		private class DtoValueConverter : IValueConverter
		{
			public bool CanMap(Type target) => target == typeof(Dto);
			public object Map(Type target, string value) => new Dto();
		}

		private class DtoValueSelector : IValueSelector
		{
			public string Select(ValueSelectorArgs args) => "dto";
		}

		private class DtoPropertySelector : IPropertySelector
		{
			public IEnumerable<PropertyDescriptor> Select(Type targetType) => Enumerable.Empty<PropertyDescriptor>();
		}

		private class UnusedValueSelector : IValueSelector
		{
			public string Select(ValueSelectorArgs args)
			{
				throw new NotSupportedException();
			}
		}

		private class UnusedPropertySelector : IPropertySelector
		{
			public IEnumerable<PropertyDescriptor> Select(Type targetType)
			{
				throw new NotSupportedException();
			}
		}
	}
}