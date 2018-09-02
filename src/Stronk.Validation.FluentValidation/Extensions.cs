using FluentValidation;
using Stronk.Dsl;

namespace Stronk.Validation.FluentValidation
{
	public static class Extensions
	{
		public static StronkConfig With<TValidator>(this ValidationExpression expression) where TValidator : global::FluentValidation.IValidator, new()
		{
			return expression.Using<object>(config =>
			{
				var validator = new TValidator();

				if (validator.CanValidateInstancesOfType(config.GetType()) == false)
					return;

				var results = validator.Validate(config);

				if (results.IsValid == false)
					throw new ValidationException(results.Errors);
			});
		}

		public static StronkConfig With<T>(this ValidationExpression expression, AbstractValidator<T> validator)
		{
			return expression.Using<T>(config => validator.ValidateAndThrow(config));
		}
	}
}
