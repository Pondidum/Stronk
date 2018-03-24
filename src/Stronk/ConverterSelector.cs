using System.Linq;
using Stronk.Policies;
using Stronk.PropertyWriters;
using Stronk.ValueConverters;

namespace Stronk
{
	public class ConverterSelector
	{
		private readonly IStronkConfig _options;

		public ConverterSelector(IStronkConfig options)
		{
			_options = options;
		}

		public IValueConverter[] Select(PropertyDescriptor property)
		{
			var validConverters = _options.ValueConverters.Where(c => c.CanMap(property.Type)).ToArray();

			if (validConverters.Any())
				return validConverters;

			_options.WriteLog("Unable to any converters for {typeName} for property {propertyName}", property.Type.Name, property.Name);

			_options.ErrorPolicy.OnConverterNotFound.Handle(new ConverterNotFoundArgs
			{
				AvailableConverters = _options.ValueConverters,
				Property = property
			});

			return new IValueConverter[0];
		}
	}
}
