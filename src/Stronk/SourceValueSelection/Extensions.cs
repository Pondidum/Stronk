using Stronk.Dsl;

namespace Stronk.SourceValueSelection
{
	public static class Extensions
	{
		public static StronkConfig PropertyNames(this MapExpression self)
		{
			return self.With(new PropertyNameSourceValueSelector());
		}
	}
}
