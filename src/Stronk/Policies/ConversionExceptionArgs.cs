using System;
using Stronk.PropertyWriters;

namespace Stronk.Policies
{
	public class ConversionExceptionArgs
	{
		public Action<string, object[]> Logger { get; set; }
		public Exception Exception { get; set; }
		public PropertyDescriptor Property { get; set; }
		public string Value { get; set; }
	}
}
