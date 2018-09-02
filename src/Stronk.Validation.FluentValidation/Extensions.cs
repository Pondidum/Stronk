using FluentValidation;
using Stronk.Dsl;

namespace Stronk.Validation.FluentValidation
{
	public static class Extensions
	{
		public static StronkConfig Using<TValidator>(this ValidationExpression expression) where TValidator : global::FluentValidation.IValidator, new()
		{
			return expression.Using<object>(config =>
			{
				var validator = new TValidator();
				var results = validator.Validate(config);

				if (results.IsValid == false)
					throw new ValidationException(results.Errors);
			});
		}
	}
}
