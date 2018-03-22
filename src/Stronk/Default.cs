using System;
using System.Collections.Generic;
using System.Globalization;
using Stronk.ConfigurationSources;
using Stronk.Policies;
using Stronk.PropertyMappers;
using Stronk.PropertyWriters;
using Stronk.ValueConverters;

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

		public static IEnumerable<IPropertyMapper> SourceValueSelectors { get; } = new IPropertyMapper[]
		{
			new PropertyNamePropertyMapper(),
		};

		public static IEnumerable<IConfigurationSource> ConfigurationSources { get; } = new IConfigurationSource[]
		{
			new AppConfigSource()
		};

		public static ErrorPolicy ErrorPolicy { get; } = new ErrorPolicy();
	}
}
