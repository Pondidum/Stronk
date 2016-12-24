namespace Stronk.Tests.DefaultConversionTests
{
	public class EnumTextConversion : TypeConversionTest<Enumeration>
	{
		public EnumTextConversion() : base(
			inputSingle: "Two",
			inputMultiple: "One,Three,Two",
			expected: Enumeration.Two,
			expectedCollection: new[] { Enumeration.One, Enumeration.Three, Enumeration.Two }
		)
		{
		}
	}
}
