using System.Linq;
using Stronk.Policies;
using Stronk.PropertyMappers;
using Stronk.PropertyWriters;

namespace Stronk
{
	public class ValueSelector
	{
		private readonly IStronkConfig _options;

		public ValueSelector(IStronkConfig options)
		{
			_options = options;
		}

		public string Select(PropertyDescriptor property)
		{
			var selectionArgs = new PropertyMapperArgs(_options.WriteLog, _options.ConfigSources, property);
			var valueToUse = _options.Mappers.Select(x => x.ValueFor(selectionArgs)).FirstOrDefault(v => v != null);

			if (valueToUse != null)
				return valueToUse;

			if (property.IsOptional)
				return string.Empty;

			_options.WriteLog("Unable to find a value for {propertyName}", property.Name);

			throw new SourceValueNotFoundException(new SourceValueNotFoundArgs
			{
				ValueSelectors = _options.Mappers,
				Property = property,
				Sources = _options.ConfigSources
			});
		}
	}
}
