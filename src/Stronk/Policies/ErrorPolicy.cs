namespace Stronk.Policies
{
	public class ErrorPolicy
	{
		public ISourceValueNotFoundPolicy OnSourceValueNotFound { get; set; }
		public IConverterNotFoundPolicy OnConverterNotFound { get; set; }
		public IConversionPolicy ConversionPolicy { get; set; }

		public ErrorPolicy()
		{
			OnSourceValueNotFound = new SourceValueNotFoundPolicy(PolicyActions.ThrowException);
			OnConverterNotFound = new ConverterNotFoundPolicy(PolicyActions.ThrowException);
			ConversionPolicy = new ConversionPolicy(ConverterExceptionPolicy.FallbackOrThrow);
		}
	}
}
