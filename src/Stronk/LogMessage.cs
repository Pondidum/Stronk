using System;
using System.Text.RegularExpressions;

namespace Stronk
{
	public class LogMessage
	{
		public string  Template { get; }
		public object[] Args { get; }

		public LogMessage(string template, object[] args)
		{
			Template = template;
			Args = args;
		}

		public override string ToString()
		{
			var rx = new Regex(@"\{(.*?)\}");
			var index = 0;
			var rendered = rx.Replace(Template, eval => Convert.ToString(Args[index++]));

			return rendered;
		}
	}
}
