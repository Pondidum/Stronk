using System.Collections.Generic;
using Stronk.PropertySelection;
using Stronk.SourceValueSelection;

namespace Stronk.Policies
{
	public class SourceValueNotFoundArgs
	{
		public IEnumerable<ISourceValueSelector> ValueSelectors { get; set; }
		public PropertyDescriptor Property { get; set; }
	}
}
