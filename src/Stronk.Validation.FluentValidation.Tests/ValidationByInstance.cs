namespace Stronk.Validation.FluentValidation.Tests
{
	public class ValidationByInstance : ValidationTests
	{
		public ValidationByInstance()
		{
			_builder.Validate.With(new TargetValidator());
		}
	}
}
