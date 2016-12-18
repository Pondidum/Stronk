using System;
using System.Collections.Generic;
using System.Linq;
using Stronk.Policies;
using Stronk.PropertySelection;
using Stronk.SourceValueSelection;
using Stronk.ValueConversion;

namespace Stronk
{
	public class StronkOptions
	{
		public List<IValueConverter> ValueConverters { get; set; }
		public List<IPropertySelector> PropertySelectors { get; set; }
		public List<ISourceValueSelector> ValueSelectors { get; set; }
		public ErrorPolicy ErrorPolicy { get; set; }

		public StronkOptions()
		{
			ValueConverters = Default.ValueConverters.ToList();
			PropertySelectors = Default.PropertySelectors.ToList();
			ValueSelectors = Default.SourceValueSelectors.ToList();

			ErrorPolicy = new ErrorPolicy();
		}
	}
}
