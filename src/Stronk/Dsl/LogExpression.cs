using System;
using System.Collections.Generic;

namespace Stronk.Dsl
{
	public class LogExpression
	{
		private readonly StronkConfig _configRoot;
		private readonly List<Action<LogMessage>> _loggers;

		public LogExpression(StronkConfig configRoot)
		{
			_configRoot = configRoot;
			_loggers = new List<Action<LogMessage>>();
		}

		public StronkConfig Using(Action<LogMessage> logger)
		{
			_loggers.Add(logger);
			return _configRoot;
		}

		internal void Write(string template, params object[] args)
		{
			_loggers.ForEach(logger => logger(new LogMessage(template, args)));
		}
	}
}
