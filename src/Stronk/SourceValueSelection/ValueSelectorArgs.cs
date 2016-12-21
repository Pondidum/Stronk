﻿using System;
using System.Collections.Specialized;
using System.Configuration;
using Stronk.PropertySelection;

namespace Stronk.SourceValueSelection
{
	public class ValueSelectorArgs
	{
		public Action<string, object[]> Logger { get; }
		public NameValueCollection AppSettings { get; }
		public ConnectionStringSettingsCollection ConnectionStrings { get; }
		public PropertyDescriptor Property { get; private set; }

		internal ValueSelectorArgs(Action<string, object[]> logger, IConfigurationSource source)
		{
			Logger = logger;
			AppSettings = source.AppSettings;
			ConnectionStrings = source.ConnectionStrings;
		}

		internal ValueSelectorArgs With(PropertyDescriptor property)
		{
			Property = property;
			return this;
		}
	}
}
