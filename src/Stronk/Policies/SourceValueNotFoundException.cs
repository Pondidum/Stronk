using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Stronk.PropertySelection;
using Stronk.SourceValueSelection;

namespace Stronk.Policies
{
	public class SourceValueNotFoundException : Exception
	{
		public SourceValueNotFoundException(SourceValueNotFoundArgs source)
			: base(BuildMessage(source))
		{
		}

		private static string BuildMessage(SourceValueNotFoundArgs descriptor)
		{
			var sb = new StringBuilder();

			var property = descriptor.Property;

			sb.AppendLine($"Unable to find a value for '{property.Type.Name}' property '{property.Name}'.");
			sb.AppendLine();

			sb.AppendLine("Tried using the following selectors:");
			foreach (var selector in descriptor.ValueSelectors)
				sb.AppendLine(selector.GetType().Name.Replace("SourceValueSelector", ""));

			sb.AppendLine();

			var appSettings = descriptor.Source.AppSettings;
			var connectionStrings = descriptor.Source.ConnectionStrings;

			if (appSettings.Any() == false && connectionStrings.Any() == false)
			{
				sb.AppendLine("There were no AppSettings or ConnectionStrings to read");
				return sb.ToString();
			}

			sb.AppendLine("The following values were available:");

			if (appSettings.Any())
			{
				sb.AppendFormat("AppSettings:");
				foreach (var key in appSettings.Keys)
					sb.AppendLine($"\t{key}");
			}

			if (connectionStrings.Any())
			{
				sb.AppendFormat("ConnectionStrings:");
				foreach (var key in connectionStrings.Keys)
					sb.AppendLine($"\t{key}");
			}

			return sb.ToString();
		}
	}
}
