using System;
using System.Linq;

namespace Stronk.Policies
{
	public class ValueConversionException : Exception
	{
		public Exception[] InnerExceptions { get; }

		public ValueConversionException(string message, Exception[] exceptions)
			: base(message, exceptions.FirstOrDefault())
		{
			InnerExceptions = exceptions.ToArray();
		}
	}
}
