namespace Stronk.Policies
{
	public class ErrorPolicy
	{
		public PolicyActions OnSourceValueNotFound { get; set; }
		public PolicyActions OnConverterNotFound { get; set; }
	}

	public enum PolicyActions
	{
		ThrowException,
		Skip
	}
}
