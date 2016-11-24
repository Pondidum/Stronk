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


		public object Map(Type target, string value)
		{
			var values = value.Split(',');

			if (IsIEnumerable(target))
			{
				return values
					.Select(val => Convert.ChangeType(val, target.GetGenericArguments()[0]))
					.ToArray();
			}

			if (IsIList(target))
			{
				return values
					.Select(val => Convert.ChangeType(val, target.GetGenericArguments()[0]))
					.ToList();
			}

			if (target.IsArray)
			{
				return values
					.Select(val =>  Convert.ChangeType(val, target.GetElementType()))
					.ToArray();
			}

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
	}
}
