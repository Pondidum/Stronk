using System;
using System.Collections.Generic;
using System.Linq;
using Stronk.Validation;

namespace Stronk.Dsl
{
	public class ValidationExpression
	{
		private readonly StronkConfig _configRoot;
		private readonly List<IValidator> _validators;

		public ValidationExpression(StronkConfig configRoot)
		{
			_configRoot = configRoot;
			_validators = new List<IValidator>();
		}

		public IEnumerable<IValidator> Validators => _validators;

		public StronkConfig Using<TConfig>(Action<TConfig> validate)
		{
			_validators.Add(new LambdaValidator(
				typeof(TConfig),
				x => validate((TConfig)x))
			);

			return _configRoot;
		}

		public StronkConfig AllSourceValuesAreUsed()
		{
			_validators.Add(new ValueSelectorValidator());
			return _configRoot;
		}
	}
}
