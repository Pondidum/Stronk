using System.Collections.Generic;
using Stronk.PropertySelection;
using Stronk.ValueConversion;
using Stronk.ValueSelection;

namespace Stronk
{
	public interface IStronkConfiguration
	{
		IEnumerable<IValueConverter> ValueConverters { get; }
		IEnumerable<IPropertySelector> PropertySelectors { get; }
		IEnumerable<ISourceValueSelector> ValueSelectors { get; }
	}
}
