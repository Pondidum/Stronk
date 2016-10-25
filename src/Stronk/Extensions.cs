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
			var converters = new IValueConverter[]
			{
				new LambdaValueConverter<Uri>(val => new Uri(val)), 
				new EnumValueConverter(),
				new FallbackValueConverter()
			};

			var properties = target
				.GetType()
				.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			var appSettings = ConfigurationManager.AppSettings;

			foreach (var property in properties)
			{
				var hasSetting = appSettings.AllKeys.Contains(property.Name);

				if (hasSetting)
				{
					var converted = converters
						.First(c => c.CanMap(property.PropertyType))
						.Map(property.PropertyType, appSettings[property.Name]);

					property.GetSetMethod(true).Invoke(target, new[] { converted });
				}
			}
		}

		public interface IValueConverter
		{
			bool CanMap(Type target);
			object Map(Type target, string value);
		}

		public class FallbackValueConverter : IValueConverter
		{
			public bool CanMap(Type target) => true;

			public object Map(Type target, string value)
			{
				return Convert.ChangeType(value, target);
			}
		}

		public class EnumValueConverter : IValueConverter
		{
			public bool CanMap(Type target) => target.IsEnum;

			public object Map(Type target, string value)
			{
				return Enum.Parse(target, value, ignoreCase: true);
			}
		}

		public class LambdaValueConverter<T> : IValueConverter
		{
			private readonly Func<string, T> _convert;

			public LambdaValueConverter(Func<string, T> convert)
			{
				_convert = convert;
			}

			public bool CanMap(Type target) => typeof(T).IsAssignableFrom(target);

			public object Map(Type target, string value)
			{
				return _convert(value);
			}
		}
	}
}
