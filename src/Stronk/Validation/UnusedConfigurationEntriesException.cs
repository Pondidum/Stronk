using System;
using System.Text;

namespace Stronk.Validation
{
	public class UnusedConfigurationEntriesException : Exception
	{
		public string[] UnusedKeys { get; }

		public UnusedConfigurationEntriesException(string[] unusedKeys)
			: base(BuildMessage(unusedKeys))
		{
			UnusedKeys = unusedKeys;
		}

		private static string BuildMessage(string[] unusedKeys)
		{
			var sb = new StringBuilder();

			sb.AppendLine("The following Keys were not used when populating the configuration object:");
			sb.AppendLine();

			foreach (var key in unusedKeys)
				sb.AppendLine($"* {key}");

			return sb.ToString();
		}
	}
}
