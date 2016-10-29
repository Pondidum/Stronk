using System.Collections.Generic;
using Stronk.PropertySelection;
using Stronk.ValueConversion;
using Stronk.ValueSelection;

namespace Stronk
{
	public interface IStronkConfiguration
	{
		IEnumerable<IValueConverter> Converters { get; }
		IEnumerable<IPropertySelector> PropertySelectors { get; }
		IEnumerable<IValueSelector> ValueSelectors { get; }
	}
}
