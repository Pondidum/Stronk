using System;
using System.Collections.Generic;
using Stronk.Validation;

namespace Stronk.Dsl
{
	public class ValidationExpression
	{
		private readonly IStronkConfig _configRoot;
		private readonly List<IValidator> _validators;

		public ValidationExpression(IStronkConfig configRoot)
		{
			_configRoot = configRoot;
			_validators = new List<IValidator>();
		}

		public IEnumerable<IValidator> Validators => _validators;

		public IStronkConfig Using<TConfig>(Action<TConfig> validate)
		{
			_validators.Add(new LambdaValidator(
				typeof(TConfig),
				x => validate((TConfig)x))
			);

			return _configRoot;
		}
	}
}
