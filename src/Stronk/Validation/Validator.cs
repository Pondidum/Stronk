using System.Linq;

namespace Stronk.Validation
{
	public class Validator
	{
		private readonly IStronkConfig _config;

		public Validator(IStronkConfig config)
		{
			_config = config;
		}

		public void Validate<TConfig>(TConfig target)
		{
			var validators = _config
				.Validators
				.Where(v => v.CanValidate<TConfig>());

			foreach (var validator in validators)
				validator.Validate(target);
		}
	}
}
