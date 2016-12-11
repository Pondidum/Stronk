namespace Stronk.Policies
{
	public class ErrorPolicy
	{
		public PolicyActions OnSourceValueNotFound { get; set; }
	}

	public enum PolicyActions
	{
		ThrowException,
		Skip
	}
}
