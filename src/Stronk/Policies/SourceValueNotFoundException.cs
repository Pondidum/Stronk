using System;
using System.Linq;
using System.Text;

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

			var allSettings = descriptor
				.Sources
				.SelectMany(source => source.GetAvailableKeys())
				.OrderBy(key => key)
				.ToArray();

			if (allSettings.Any() == false)
			{
				sb.AppendLine("There were no Settings to read");
				return sb.ToString();
			}

			sb.AppendLine("The following settings were available:");
			sb.AppendLine();

			if (allSettings.Any())
			{
				foreach (var key in allSettings)
					sb.AppendLine($"{key}");
			}

			return sb.ToString();
		}
	}
}
