using System;

namespace Stronk.Tests.DefaultConversionTests
{
	public class TimeSpanConversion : TypeConversionTest<TimeSpan>
	{
		public TimeSpanConversion() : base(
			inputSingle: "1.02:03:04.0050000",
			inputMultiple: "1.02:03:04.0050000,2:31:48",
			expected: new TimeSpan(1, 2, 3, 4, 5),
			expectedCollection: new[] { new TimeSpan(1, 2, 3, 4, 5), new TimeSpan(2, 31, 48), }
		)
		{
		}
	}
}
