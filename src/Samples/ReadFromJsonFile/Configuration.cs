using System;

namespace ReadFromJsonFile
{
	public class Configuration
	{
		public Uri Callback { get; set; }
		public TimeSpan Timeout { get; set; }
	}
}
