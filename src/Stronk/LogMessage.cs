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
	}
}
