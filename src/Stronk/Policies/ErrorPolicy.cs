namespace Stronk.Policies
{
	public class ErrorPolicy
	{
		public PolicyActions OnSourceValueNotFound { get; set; }
		public PolicyActions OnConverterNotFound { get; set; }
		public ConverterExceptionPolicy OnConverterException { get; set; }
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
