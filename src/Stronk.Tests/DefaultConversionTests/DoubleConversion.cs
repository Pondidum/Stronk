namespace Stronk.Tests.DefaultConversionTests
{
	public class DoubleConversion : TypeConversionTest<double>
	{
		public DoubleConversion() : base(
			inputSingle: "123.45",
			inputMultiple: "123.45,6,9.01",
			expected: 123.45,
			expectedCollection: new[] { 123.45, 6, 9.01 }
		)
		{
		}
	}
}