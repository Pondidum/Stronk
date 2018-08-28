using System;

namespace ReadFromConsul
{
	public class Configuration
	{
		public TimeSpan Timeout { get; set; }
		public Uri Callback { get; set; }
	}
}
