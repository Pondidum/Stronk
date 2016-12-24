namespace Stronk.Tests.DefaultConversionTests
{
	public class DecimalConversion : TypeConversionTest<decimal>
	{
		public DecimalConversion() : base(
			inputSingle: "123.456",
			inputMultiple: "123.45,76.4",
			expected: 123.456M,
			expectedCollection: new[] { 123.45M, 76.4M }
		)
		{
		}
	}
}
