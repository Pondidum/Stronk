using System;

namespace Stronk.Dsl
{
	public interface ILogExpression
	{
		StronkConfig Using(Action<LogMessage> logger);
	}
}
