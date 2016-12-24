using System;

namespace Stronk.Tests.DefaultConversionTests
{
	public class UrlConversion : TypeConversionTest<Uri>
	{
		public UrlConversion() : base(
			inputSingle: "https://example.com/some/path?option=very_yes",
			inputMultiple: "https://example.com/some/path?option=very_yes,http://www.example.org",
			expected: new Uri("https://example.com/some/path?option=very_yes"),
			expectedCollection: new[]
			{
				new Uri("https://example.com/some/path?option=very_yes"),
				new Uri("http://www.example.org"),
			}
		)
		{
		}
	}
}
