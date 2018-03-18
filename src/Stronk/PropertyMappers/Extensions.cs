using Stronk.Dsl;

namespace Stronk.PropertyMappers
{
	public static class Extensions
	{
		public static StronkConfig PropertyNames(this MapExpression self)
		{
			return self.With(new PropertyNamePropertyMapper());
		}
	}
}
