namespace Stronk.Validation.FluentValidation.Tests
{
	public class ValidationByGenericType : ValidationTests
	{
		public ValidationByGenericType()
		{
			_builder.Validate.With<TargetValidator>();
		}
	}
}
