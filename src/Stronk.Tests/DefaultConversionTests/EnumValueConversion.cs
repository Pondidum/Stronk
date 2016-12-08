namespace Stronk.Tests.DefaultConversionTests
{
	public class EnumValueConversion : TypeConversionTest<Enumeration>
	{
		public EnumValueConversion() : base(
			inputSingle: "3",
			inputMultiple: "2,3,1",
			expected: Enumeration.Three,
			expectedCollection: new[] { Enumeration.Two, Enumeration.Three, Enumeration.One }
		)
		{
		}
	}
}