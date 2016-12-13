using System;

namespace Stronk
{
	public class ValueConversionException : Exception
	{
		public Exception[] InnerExceptions { get; private set; }
	}
}
