using System.Collections.Generic;
using System.Linq;
using Stronk.ValueConversion;

namespace Stronk.Dsl
{
	public class ConversionExpression
	{
		private readonly StronkConfig _configRoot;
		private readonly List<IValueConverter> _converters;
		private bool _onlySpecifiedConverters;

		public ConversionExpression(StronkConfig configRoot)
		{
			_configRoot = configRoot;
			_converters = new List<IValueConverter>();
			_onlySpecifiedConverters = false;
		}

		public StronkConfig Using(params IValueConverter[] converters)
		{
			_converters.AddRange(converters);
			return _configRoot;
		}

		public StronkConfig UsingOnly(params IValueConverter[] converters)
		{
			_converters.AddRange(converters);
			_onlySpecifiedConverters = true;
			return _configRoot;
		}

		internal IEnumerable<IValueConverter> Converters => _onlySpecifiedConverters
			? _converters
			: _converters.Concat(Default.ValueConverters);
	}
}
