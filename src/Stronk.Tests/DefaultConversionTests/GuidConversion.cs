using System;

namespace Stronk.Tests.DefaultConversionTests
{
	public class GuidConversion : TypeConversionTest<Guid>
	{
		public GuidConversion() : base(
			inputSingle: "7AB987EE-0ACE-45FF-B6CA-DE75091D045D",
			inputMultiple: "7AB987EE-0ACE-45FF-B6CA-DE75091D045D,1D19B692-6115-41FF-AB78-7731E3BECE21",
			expected: Guid.Parse("7AB987EE-0ACE-45FF-B6CA-DE75091D045D"),
			expectedCollection: new[]
			{
				Guid.Parse("7AB987EE-0ACE-45FF-B6CA-DE75091D045D"),
				Guid.Parse("1D19B692-6115-41FF-AB78-7731E3BECE21")
			}
		)
		{
		}
	}
}
