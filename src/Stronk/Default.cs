﻿using System;
using System.Globalization;
using Stronk.ConfigurationSourcing;
using Stronk.PropertyWriters;
using Stronk.SourceValueSelection;
using Stronk.ValueConversion;

namespace Stronk
{
	public class Default
	{
		public static IValueConverter[] ValueConverters => new IValueConverter[]
		{
			new LambdaValueConverter<Uri>(val => new Uri(val)),
			new LambdaValueConverter<Guid>(Guid.Parse),
			new LambdaValueConverter<TimeSpan>(TimeSpan.Parse),
			new LambdaValueConverter<DateTime>(val => DateTime.Parse(val, null, DateTimeStyles.RoundtripKind)),
			new EnumValueConverter(),
			new CsvValueConverter(),
			new FallbackValueConverter()
		};

		public static IPropertyWriter[] PropertyWriters => new IPropertyWriter[]
		{
			new PrivateSetterPropertyWriter(),
			new BackingFieldPropertyWriter(),
		};

		public static ISourceValueSelector[] SourceValueSelectors => new ISourceValueSelector[]
		{
			new PropertyNameSourceValueSelector(),
		};

		public static IConfigurationSource[] ConfigurationSources => new IConfigurationSource[]
		{
			new AppConfigSource()
		};
	}
}
