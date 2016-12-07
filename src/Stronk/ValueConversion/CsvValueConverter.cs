using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
				return CastArray(targetType, convertedValues.ToArray());

			if (e.Target.IsArray)
				return CastArray(targetType, convertedValues.ToArray());

			if (IsList(e.Target))
				return GenerateList(targetType, convertedValues);

			throw new NotSupportedException($"Unable to cast to '{e.Target.Name}', as only arrays, IEnumerable<T> an IList<T> are supported at the moment");
		}

		private static object GenerateList(Type targetType, IEnumerable<object> convertedValues)
		{
			var castMethod = typeof(Enumerable).GetMethod("Cast", BindingFlags.Static | BindingFlags.Public);
			var genericCast = castMethod.MakeGenericMethod(targetType);

			var toListMethod = typeof(Enumerable).GetMethod("ToList", BindingFlags.Static | BindingFlags.Public);
			var genericToList = toListMethod.MakeGenericMethod(targetType);

			var casted = genericCast.Invoke(null, new object[] { convertedValues });
			var list = genericToList.Invoke(null, new[] { casted });

			return list;
		}

		private static bool IsIEnumerable(Type target)
		{
			return target.IsGenericType && target.GetGenericTypeDefinition() == typeof(IEnumerable<>);
		}

		private static bool IsList(Type target)
		{
			return target.IsGenericType && (
				target.GetGenericTypeDefinition() == typeof(IList<>)
				||
				target.GetGenericTypeDefinition() == typeof(List<>)
			);
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
