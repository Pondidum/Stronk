using System.Collections.Generic;
using System.Linq;
using Stronk.PropertyWriters;

namespace Stronk.Dsl
{
	public class WriterExpression
	{
		private readonly StronkConfig _configRoot;
		private readonly List<IPropertyWriter> _writers;
		
		public WriterExpression(StronkConfig configRoot)
		{
			_configRoot = configRoot;
			_writers = new List<IPropertyWriter>();
		}
		
		public StronkConfig To(IPropertyWriter writer)
		{
			_writers.Add(writer);
			return _configRoot;
		}

		internal IEnumerable<IPropertyWriter> Writers => _writers.Any()
			? _writers
			: Default.PropertyWriters;
	}
}