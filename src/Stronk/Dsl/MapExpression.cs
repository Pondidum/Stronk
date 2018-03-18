using System.Collections.Generic;
using System.Linq;
using Stronk.PropertyMappers;

namespace Stronk.Dsl
{
	public class MapExpression
	{
		private readonly StronkConfig _configRoot;
		private readonly List<IPropertyMapper> _selectors;

		public MapExpression(StronkConfig configRoot)
		{
			_configRoot = configRoot;
			_selectors = new List<IPropertyMapper>();
		}
		
		public StronkConfig With(IPropertyMapper selector)
		{
			_selectors.Add(selector);
			return _configRoot;
		}

		internal IEnumerable<IPropertyMapper> Mappers => _selectors.Any()
			? _selectors
			: Default.SourceValueSelectors;
	}
}