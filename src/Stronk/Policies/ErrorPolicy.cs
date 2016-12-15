namespace Stronk.Policies
{
	public class ErrorPolicy
	{
		public PolicyActions OnSourceValueNotFound { get; set; }
		public PolicyActions OnConverterNotFound { get; set; }
		public IConversionPolicy ConversionPolicy { get; set; }

		public ErrorPolicy()
		{
			OnSourceValueNotFound = PolicyActions.ThrowException;
			OnConverterNotFound = PolicyActions.ThrowException;
			ConversionPolicy = new ConversionPolicy(ConverterExceptionPolicy.FallbackOrThrow);
		}
	}

	public enum ConverterExceptionPolicy
	{
		ThrowException,
		FallbackOrThrow,
		FallbackOrSkip,
		Skip
	}

	public enum PolicyActions
	{
		ThrowException,
		Skip
	}
}
