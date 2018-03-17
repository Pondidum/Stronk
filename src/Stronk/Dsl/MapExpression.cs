using System.Collections.Generic;
using System.Linq;
using Stronk.SourceValueSelection;

namespace Stronk.Dsl
{
	public class MapExpression
	{
		private readonly StronkConfig _configRoot;
		private readonly List<ISourceValueSelector> _selectors;

		public MapExpression(StronkConfig configRoot)
		{
			_configRoot = configRoot;
			_selectors = new List<ISourceValueSelector>();
		}
		
		public StronkConfig With(ISourceValueSelector selector)
		{
			_selectors.Add(selector);
			return _configRoot;
		}

		internal IEnumerable<ISourceValueSelector> Selectors => _selectors.Any()
			? _selectors
			: Default.SourceValueSelectors;
	}
}