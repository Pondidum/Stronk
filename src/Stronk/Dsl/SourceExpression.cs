using System.Collections.Generic;
using System.Linq;
using Stronk.ConfigurationSourcing;

namespace Stronk.Dsl
{
	public class SourceExpression
	{
		private readonly StronkConfig _configRoot;
		private readonly List<IConfigurationSource> _sources;

		public SourceExpression(StronkConfig configRoot)
		{
			_configRoot = configRoot;
			_sources = new List<IConfigurationSource>();
		}

		public StronkConfig Source(IConfigurationSource source)
		{
			_sources.Add(source);
			return _configRoot;
		}

		internal IEnumerable<IConfigurationSource> Sources => _sources.Any()
			? _sources
			: Default.ConfigurationSources;
	}
}
