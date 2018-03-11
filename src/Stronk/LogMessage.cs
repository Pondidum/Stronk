using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;

namespace Stronk
{
	public class LogMessage
	{
		private static readonly Regex MergeFields = new Regex(@"\{(.*?)\}");

		public string Template { get; }
		public object[] Args { get; }

		public LogMessage(string template, object[] args)
		{
			Template = template;
			Args = args;
		}

		private string Render(object item)
		{
			if (item is string)
				return (string)item;

			if (item is IEnumerable)
				return string.Join(", ", ((IEnumerable)item).Cast<object>());

			return Convert.ToString(item);
		}

		public override string ToString()
		{
			var index = 0;
			var rendered = MergeFields.Replace(Template, eval => Render(Args[index++]));

			return rendered;
		}
	}
}
