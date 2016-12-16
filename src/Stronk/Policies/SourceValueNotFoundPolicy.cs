using System.Collections.Generic;
using Stronk.SourceValueSelection;

namespace Stronk.Policies
{
	public class SourceValueNotFoundPolicy : ISourceValueNotFoundPolicy
	{
		private readonly PolicyActions _action;

		public SourceValueNotFoundPolicy(PolicyActions action)
		{
			_action = action;
		}

		public void Handle(IEnumerable<ISourceValueSelector> valueSelectors, ValueSelectorArgs selectorArgs)
		{
			if (_action == PolicyActions.ThrowException)
				throw new SourceValueNotFoundException(valueSelectors, selectorArgs.Property);
		}
	}
}
