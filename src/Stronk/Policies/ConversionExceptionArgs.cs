using System;

namespace Stronk.Policies
{
	public class ConversionExceptionArgs
	{
		public Action<LogMessage> Logger { get; set; }
		public Exception Exception { get; set; }
	}
}
