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

			if (IsIEnumerable(e.Target))
			{
				return values
					.Select(val => Convert.ChangeType(val, e.Target.GetGenericArguments()[0]))
					.ToArray();
			}

			if (IsIList(e.Target))
			{
				return values
					.Select(val => Convert.ChangeType(val, e.Target.GetGenericArguments()[0]))
					.ToList();
			}

			if (e.Target.IsArray)
			{
				return values
					.Select(val =>  Convert.ChangeType(val, e.Target.GetElementType()))
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
