using System;
using System.Linq;
using Shouldly;
using Stronk.ValueConversion;
using Xunit;

namespace Stronk.Tests.DefaultConversionTests
{
	public class DefaultConversionTests
	{
		private TTarget Convert<TTarget>(string sourceValue)
		{
			var allConverters = Default.ValueConverters;
			var converter = allConverters.First(c => c.CanMap(typeof(TTarget)));

			return (TTarget)converter.Map(new ValueConverterArgs(
				allConverters.Where(x => x != converter),
				typeof(TTarget),
				sourceValue));
		}


		[Fact]
		public void TimeSpan_value_is_converted() => Convert<TimeSpan>("1.02:03:04.0050000").ShouldBe(new TimeSpan(1, 2, 3, 4, 5));

	}
}
