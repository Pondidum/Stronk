using System.Collections.Generic;
using Stronk.SourceValueSelection;

namespace Stronk.Policies
{
	public interface ISourceValueNotFoundPolicy
	{
		void Handle(IEnumerable<ISourceValueSelector> valueSelectors, ValueSelectorArgs selectorArgs);
	}
}
