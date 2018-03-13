using Stronk.ValueConversion;

namespace Stronk.Dsl
{
	public interface IConversionExpression
	{
		StronkConfig Using(params IValueConverter[] converters);
		StronkConfig UsingOnly(params IValueConverter[] converters);
	}
}
