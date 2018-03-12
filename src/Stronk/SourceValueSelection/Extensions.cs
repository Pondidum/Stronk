using Stronk.Dsl;

namespace Stronk.SourceValueSelection
{
	public static class Extensions
	{
		public static StronkConfig PropertyNames(this IMapExpression self)
		{
			return self.With(new PropertyNameSourceValueSelector());
		}
	}
}
