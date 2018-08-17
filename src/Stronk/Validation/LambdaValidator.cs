using System;

namespace Stronk.Validation
{
	public class LambdaValidator : IValidator
	{
		private readonly Type _type;
		private readonly Action<object> _validate;

		public LambdaValidator(Type type, Action<object> validate)
		{
			_type = type;
			_validate = validate;
		}

		public bool CanValidate<T>() => _type.IsAssignableFrom(typeof(T));
		public void Validate<T>(T target) => _validate(target);
	}
}
