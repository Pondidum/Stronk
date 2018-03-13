using System;
using System.Collections.Generic;
using System.Globalization;
using Stronk.ConfigurationSourcing;
using Stronk.Policies;
using Stronk.PropertyWriters;
using Stronk.SourceValueSelection;
using Stronk.ValueConversion;

namespace Stronk
{
	public class Default
	{
		public static IEnumerable<IValueConverter> ValueConverters { get; } = new IValueConverter[]
		{
			new LambdaValueConverter<Uri>(val => new Uri(val)),
			new LambdaValueConverter<Guid>(Guid.Parse),
			new LambdaValueConverter<TimeSpan>(TimeSpan.Parse),
			new LambdaValueConverter<DateTime>(val => DateTime.Parse(val, null, DateTimeStyles.RoundtripKind)),
			new EnumValueConverter(),
			new CsvValueConverter(),
			new FallbackValueConverter()
		};

		public static IEnumerable<IPropertyWriter> PropertyWriters { get; } = new IPropertyWriter[]
		{
			new PrivateSetterPropertyWriter(),
			new BackingFieldPropertyWriter(),
		};

		public static IEnumerable<ISourceValueSelector> SourceValueSelectors { get; } = new ISourceValueSelector[]
		{
			new PropertyNameSourceValueSelector(),
		};

		public static IEnumerable<IConfigurationSource> ConfigurationSources { get; } = new IConfigurationSource[]
		{
			new AppConfigSource()
		};

		public static ErrorPolicy ErrorPolicy { get; } = new ErrorPolicy();
	}
}
