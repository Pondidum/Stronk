using System;
using System.Collections.Generic;
using Stronk.PropertySelection;
using Stronk.SourceValueSelection;
using Stronk.ValueConversion;

namespace Stronk
{
	public class Default
	{
		public static IValueConverter[] ValueConverters { get; } = {
			new LambdaValueConverter<Uri>(val => new Uri(val)),
			new LambdaValueConverter<Guid>(Guid.Parse),
			new LambdaValueConverter<TimeSpan>(TimeSpan.Parse),
			new EnumValueConverter(),
			new CsvValueConverter(),
			new FallbackValueConverter()
		};

		public static IPropertySelector[] PropertySelectors { get; } = {
			new PrivateSetterPropertySelector(),
			new BackingFieldPropertySelector(),
		};

		public static ISourceValueSelector[] SourceValueSelectors { get; } = {
			new PropertyNameSourceValueSelector(),
		};
	}
}