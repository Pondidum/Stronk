using Stronk.ConfigurationSourcing;

namespace Stronk.Dsl
{
	public interface ISourceExpression
	{
		StronkConfig Source(IConfigurationSource source);
	}
}
