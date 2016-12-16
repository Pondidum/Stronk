using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stronk.PropertySelection;
using Stronk.SourceValueSelection;

namespace Stronk
{
	public class SourceValueNotFoundException : Exception
	{
		public SourceValueNotFoundException(IEnumerable<ISourceValueSelector> valueSelectors, PropertyDescriptor property)
			: base(BuildMessage(valueSelectors, property))
		{
		}

		private static string BuildMessage(IEnumerable<ISourceValueSelector> valueSelectors, PropertyDescriptor property)
		{
			var sb = new StringBuilder();

			sb.AppendLine($"Unable to find a value for property '{property.Name}' using the following selectors:");
			sb.AppendLine();

			foreach (var selector in valueSelectors)
				sb.AppendLine(selector.GetType().Name);

			return sb.ToString();
		}
	}
}
