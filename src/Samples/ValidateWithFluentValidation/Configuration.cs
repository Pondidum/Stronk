using System;

namespace ValidateWithFluentValidation
{
	public class Configuration
	{
		public TimeSpan Timeout { get; set; }
		public Uri Callback { get; set; }
	}
}
