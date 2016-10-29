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
			var converters = config.Converters.Select(c => c.GetType());
			var propertySelectors = config.PropertySelectors.Select(c => c.GetType());
			var valueSelectors = config.ValueSelectors.Select(t => t.GetType());

			config.ShouldSatisfyAllConditions(
				() => converters.ShouldBe(new[] { typeof(LambdaValueConverter<Uri>), typeof(EnumValueConverter), typeof(FallbackValueConverter) }),
				() => propertySelectors.ShouldBe(new[] { typeof(PrivateSetterPropertySelector), typeof(BackingFieldPropertySelector) }),
				() => valueSelectors.ShouldBe(new[] { typeof(PropertyNameValueSelector) })
			);
		}
	}
}