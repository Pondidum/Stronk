namespace Stronk.Policies
{
	public class ErrorPolicy
	{
		public ISourceValueNotFoundPolicy OnSourceValueNotFound { get; set; }
		public IConverterNotFoundPolicy OnConverterNotFound { get; set; }
		public IConversionExceptionPolicy ConversionExceptionPolicy { get; set; }

		public ErrorPolicy()
		{
			OnSourceValueNotFound = new SourceValueNotFoundPolicy(PolicyActions.ThrowException);
			OnConverterNotFound = new ConverterNotFoundPolicy(PolicyActions.ThrowException);
			ConversionExceptionPolicy = new ConversionExceptionPolicy(ConverterExceptionPolicy.FallbackOrThrow);
		}
	}
}
