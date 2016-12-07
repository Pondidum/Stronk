using System;
using System.Collections.Generic;
using System.Linq;

namespace Stronk.ValueConversion
{
	public class CsvValueConverter : IValueConverter
	{
		public bool CanMap(Type target)
		{
			if (target == typeof(string))
				return false;

			if (IsIEnumerable(target))
				return true;

			return GetGenericInterfaces(target)
				.Contains(typeof(IEnumerable<>));
		}

		public object Map(ValueConverterArgs e)
		{
			var values = e.Input.Split(',');
			var converters = e.OtherConverters.ToArray();

			var targetType = e.Target.IsGenericType
				? e.Target.GetGenericArguments()[0]
				: e.Target.GetElementType();

			var converter = converters
				.First(c => c.CanMap(targetType));

			var convertedValues = values
				.Select(val => converter.Map(new ValueConverterArgs(converters, targetType, val)));

			if (IsIEnumerable(e.Target))
				return convertedValues.ToArray();

			if (IsIList(e.Target))
				return convertedValues.ToList();

			if (e.Target.IsArray)
				return CastArray(targetType, convertedValues.ToArray());

			throw new NotSupportedException("Only arrays, IEnumerable<T> an IList<T> are supported at the moment");
		}

		private static bool IsIEnumerable(Type target)
		{
			return target.IsGenericType && target.GetGenericTypeDefinition() == typeof(IEnumerable<>);
		}

		private static bool IsIList(Type target)
		{
			return target.IsGenericType && target.GetGenericTypeDefinition() == typeof(IList<>);
		}

		private static IEnumerable<Type> GetGenericInterfaces(Type target)
		{
			return target
				.GetInterfaces()
				.Where(i => i.IsGenericType)
				.Select(i => i.GetGenericTypeDefinition());
		}

		private static Array CastArray(Type target, object[] input)
		{
			Array dest = Array.CreateInstance(target, input.Length);
			Array.Copy(input, dest, input.Length);

			return dest;
		}
	}
}
