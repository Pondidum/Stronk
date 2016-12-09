using System;

namespace Stronk.Tests.DefaultConversionTests
{
	public class DateConversion : TypeConversionTest<DateTime>
	{
		public DateConversion() : base(
			inputSingle: "2016-12-09",
			inputMultiple: "2016-12-09,2016-11-17",
			expected: new DateTime(2016, 12, 09),
			expectedCollection: new[]
			{
				new DateTime(2016, 12, 09),
				new DateTime(2016, 11, 17)
			}
		)
		{
		}
	}

	public class DateTimeOffsetConversion : TypeConversionTest<DateTime>
	{
		public DateTimeOffsetConversion() : base(
			inputSingle: "2016-12-09T21:25:51+00:00",
			inputMultiple: "2016-12-09T21:25:51+00:00,2016-12-09T21:29:09+00:00",
			expected: new DateTime(2016, 12, 09, 21, 25, 51).ToLocalTime(),
			expectedCollection: new[]
			{
				new DateTime(2016, 12, 09, 21, 25, 51).ToLocalTime(),
				new DateTime(2016, 12, 09, 21, 29, 09).ToLocalTime()
			}
		)
		{
		}
	}

	public class DateTimeConversion : TypeConversionTest<DateTime>
	{
		public DateTimeConversion() : base(
			inputSingle: "2016-12-09T21:25:51Z",
			inputMultiple: "2016-12-09T21:25:51Z,2016-12-09T21:29:09Z",
			expected: new DateTime(2016, 12, 09, 21, 25, 51),
			expectedCollection: new[]
			{
				new DateTime(2016, 12, 09, 21, 25, 51),
				new DateTime(2016, 12, 09, 21, 29, 09)
			}
		)
		{
		}
	}
}
