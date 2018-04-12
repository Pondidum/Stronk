using System;
using System.Collections.Generic;
using System.Linq;
using Stronk.ConfigurationSources;

namespace Stronk
{
	public static class Extensions
	{
		public static void FromAppConfig(this object target)
		{
			new StronkConfig()
				.From.AppSettings()
				.ApplyTo(target);
		}

		public static IEnumerable<string> SelectTypeNames(this IEnumerable<object> instances)
		{
			return instances.Select(instance => RecurseTypeName(instance is Type ? (Type)instance : instance.GetType()));
		}

		private static string RecurseTypeName(Type type)
		{
			if (type.IsConstructedGenericType == false)
				return type.Name;

			var typeName = type.Name.Substring(0, type.Name.IndexOf("`"));
			var arguments = string.Join(", ", type.GetGenericArguments().Select(RecurseTypeName));

			return $"{typeName}<{arguments}>";
		}
	}
}
