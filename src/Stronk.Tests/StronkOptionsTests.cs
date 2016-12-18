using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Stronk.PropertySelection;
using Stronk.SourceValueSelection;
using Stronk.ValueConversion;
using Xunit;

namespace Stronk.Tests
{
	public class StronkOptionsTests
	{
		private readonly StronkOptions _config;

		public StronkOptionsTests()
		{
			_config = new StronkOptions();
		}

		[Fact]
		public void By_default_the_collections_are_initialised()
		{
			var converters = _config.ValueConverters.Select(c => c.GetType());
			var propertySelectors = _config.PropertySelectors.Select(c => c.GetType());
			var valueSelectors = _config.ValueSelectors.Select(t => t.GetType());

			_config.ShouldSatisfyAllConditions(
				() => converters.ShouldNotBeEmpty(),
				() => propertySelectors.ShouldNotBeEmpty(),
				() => valueSelectors.ShouldNotBeEmpty()
			);
		}
	}
}