using Stronk.SourceValueSelection;

namespace Stronk.Dsl
{
	public interface IMapExpression
	{
		StronkConfig With(ISourceValueSelector selector);
	}
}