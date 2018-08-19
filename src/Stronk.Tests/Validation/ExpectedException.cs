using System;

namespace Stronk.Tests.Validation
{
	public class ExpectedException : Exception
	{
		public ExpectedException() : base("This was supposed to be thrown")
		{
		}
	}
}
