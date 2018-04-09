using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Stronk.PropertyWriters
{
	public class FallbackPropertyWriter : IPropertyWriter
	{
		private readonly IEnumerable<IPropertyWriter> _others;

		public FallbackPropertyWriter(IEnumerable<IPropertyWriter> others)
		{
			_others = others;
		}

		public IEnumerable<PropertyDescriptor> Select(PropertyWriterArgs args) => _others
			.SelectMany(writer => writer.Select(args))
			.GroupBy(prop => prop.Name)
			.Select(propertyGroup => new FallbackDescriptor(propertyGroup));

		private class FallbackDescriptor : PropertyDescriptor
		{
			private readonly IGrouping<string, PropertyDescriptor> _propertyGroup;

			public FallbackDescriptor(IGrouping<string, PropertyDescriptor> propertyGroup)
				: base(propertyGroup.Key, propertyGroup.First().Type)
			{
				_propertyGroup = propertyGroup;
			}

			public override void Assign(object target, object value)
			{
				var exceptions = new List<Exception>();
				foreach (var property in _propertyGroup)
				{
					var result = Try(() => property.Assign(target, value));

					if (result.Successful)
						return;

					exceptions.Add(result.Exception);
				}

				throw new AggregateException(exceptions);
			}

			private class Result
			{
				public bool Successful { get; private set; }
				public Exception Exception { get; private set; }

				public static Result Success() => new Result { Successful = true };
				public static Result Failure(Exception ex) => new Result { Exception = ex };
			}

			private Result Try(Action action)
			{
				try
				{
					action();
					return Result.Success();
				}
				catch (TargetInvocationException e)
				{
					return Result.Failure(e.InnerException);
				}
				catch (Exception e)
				{
					return Result.Failure(e);
				}
			}
		}
	}
}
