using System;

namespace Stronk.Tests.TestUtils
{
	public class ShouldNotBeThrownException : Exception
	{
		public ShouldNotBeThrownException() : base("This should not have been thrown")
		{
		}
	}
}

