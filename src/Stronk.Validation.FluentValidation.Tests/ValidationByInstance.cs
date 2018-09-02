namespace Stronk.Validation.FluentValidation.Tests
{
	public class ValidationByInstance : ValidationTests
	{
		public ValidationByInstance()
		{
			_builder.Validate.Using(new TargetValidator());
		}
	}
}
