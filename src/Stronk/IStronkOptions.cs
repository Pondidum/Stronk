using System.Collections.Generic;
using Stronk.PropertySelection;
using Stronk.SourceValueSelection;
using Stronk.ValueConversion;

namespace Stronk
{
	public interface IStronkOptions
	{
		IEnumerable<IValueConverter> ValueConverters { get; }
		IEnumerable<IPropertySelector> PropertySelectors { get; }
		IEnumerable<ISourceValueSelector> ValueSelectors { get; }
	}
}
