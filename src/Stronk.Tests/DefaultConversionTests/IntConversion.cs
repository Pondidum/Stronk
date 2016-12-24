namespace Stronk.Tests.DefaultConversionTests
{
	public class IntConversion : TypeConversionTest<int>
	{
		public IntConversion() : base(
			inputSingle: "123",
			inputMultiple: "123,456,789",
			expected: 123,
			expectedCollection: new[] { 123, 456, 789 }
		)
		{
		}
	}
}
