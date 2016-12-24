namespace Stronk.Tests.DefaultConversionTests
{
	public class StringConversion : TypeConversionTest<string>
	{
		public StringConversion() : base(
			inputSingle: "hEllo",
			inputMultiple: "hEllo,over,there",
			expected: "hEllo",
			expectedCollection: new[] { "hEllo", "over", "there" }
		)
		{ }
	}
}
