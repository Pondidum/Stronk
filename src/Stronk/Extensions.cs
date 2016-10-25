using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace Stronk
{
	public static class Extensions
	{
		public static void FromAppConfig(this object target)
		{
			var properties = target
				.GetType()
				.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			var appSettings = ConfigurationManager.AppSettings;

			foreach (var property in properties)
			{
				var hasSetting = appSettings.AllKeys.Contains(property.Name);

				if (hasSetting)
				{
					var value = appSettings[property.Name];

					var converted = property.PropertyType.IsEnum
						? Enum.Parse(property.PropertyType, value, ignoreCase: true)
						: Convert.ChangeType(value, property.PropertyType);

					property.GetSetMethod(true).Invoke(target, new[] { converted });
				}
			}
		}
	}
}
