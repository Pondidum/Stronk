using System;
using System.Linq;

namespace Stronk
{
	public class ValueConversionException : Exception
	{
		public Exception[] InnerExceptions { get; private set; }

		public ValueConversionException(string message, Exception[] exceptions)
			: base(message, exceptions.FirstOrDefault())
		{
			InnerExceptions = exceptions.ToArray();
		}
	}
}
