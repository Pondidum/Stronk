namespace Stronk.Policies
{
	public interface IConversionExceptionPolicy
	{
		void BeforeConversion(ConversionExceptionBeforeArgs args);
		void OnConversionException(ConversionExceptionArgs args);
		void AfterConversion(ConversionExceptionAfterArgs args);
	}
}
