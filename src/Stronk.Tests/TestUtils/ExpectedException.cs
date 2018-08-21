using System;

namespace Stronk.Tests.TestUtils
{
	public class ExpectedException : Exception
	{
		public ExpectedException() : base("This was supposed to be thrown")
		{
		}
	}
}
