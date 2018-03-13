using Stronk.Policies;

namespace Stronk.Dsl
{
	public interface IErrorPolicyExpression
	{
		StronkConfig Using(ErrorPolicy errorPolicy);
	}
}
