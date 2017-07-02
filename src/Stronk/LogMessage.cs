using System;
using System.Text.RegularExpressions;

namespace Stronk
{
	public class LogMessage
	{
		private static readonly Regex MergeFields = new Regex(@"\{(.*?)\}");
		
		public string  Template { get; }
		public object[] Args { get; }

		public LogMessage(string template, object[] args)
		{
			Template = template;
			Args = args;
		}

		public override string ToString()
		{
			var index = 0;
			var rendered = MergeFields.Replace(Template, eval => Convert.ToString(Args[index++]));

			return rendered;
		}
	}
}
