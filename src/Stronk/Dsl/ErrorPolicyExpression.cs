using Stronk.Policies;

namespace Stronk.Dsl
{
	public class ErrorPolicyExpression
	{
		private readonly StronkConfig _configRoot;
		private ErrorPolicy _errorPolicy;

		public ErrorPolicyExpression(StronkConfig configRoot)
		{
			_configRoot = configRoot;
		}

		public StronkConfig Using(ErrorPolicy errorPolicy)
		{
			_errorPolicy = errorPolicy;
			return _configRoot;
		}

		internal ErrorPolicy Policy => _errorPolicy ?? Default.ErrorPolicy;
	}
}
