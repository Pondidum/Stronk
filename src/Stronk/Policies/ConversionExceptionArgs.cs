using System;
using Stronk.PropertySelection;

namespace Stronk.Policies
{
	public class ConversionExceptionArgs
	{
		public Action<LogMessage> Logger { get; set; }
		public Exception Exception { get; set; }
		public PropertyDescriptor Property { get; set; }
		public string Value { get; set; }
	}
}
