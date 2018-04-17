using System;
using System.Linq;
using Stronk.ValueConverters;

namespace Stronk.ValueConverters
{
	public class NullableValueConverter : IValueConverter
	{
		public bool CanMap(Type target) =>
			target.IsGenericType &&
			target.GetGenericTypeDefinition() == typeof(Nullable<>);

		public object Map(ValueConverterArgs e)
		{
			if (string.IsNullOrEmpty(e.Input))
				return null;

			var wrappedType = e.Target.GetGenericArguments().Single();
			var otherConverters = e.OtherConverters.ToArray();

			var converter = otherConverters.First(c => c.CanMap(wrappedType));
			var value = converter.Map(new ValueConverterArgs(e.Logger, otherConverters, wrappedType, e.Input));

			return value;
		}
	}
}
