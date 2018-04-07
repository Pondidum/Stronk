using System;
using System.Collections.Generic;
using System.Linq;

namespace Stronk.PropertyWriters
{
	public class FallbackPropertyWriter : IPropertyWriter
	{
		private readonly IEnumerable<IPropertyWriter> _others;

		public FallbackPropertyWriter(IEnumerable<IPropertyWriter> others)
		{
			_others = others;
		}

		public IEnumerable<PropertyDescriptor> Select(PropertyWriterArgs args)
		{
			var allProperties = _others
				.SelectMany(writer => writer.Select(args))
				.GroupBy(prop => prop.Name);

			foreach (var propertyGroup in allProperties)
			{
				yield return new PropertyDescriptor
				{
					Name = propertyGroup.Key,
					Type = propertyGroup.First().Type,
					Assign = (target, value) =>
					{
						var exceptions = new List<Exception>();
						var last = propertyGroup.Last();
						foreach (var descriptor in propertyGroup)
						{
							try
							{
								descriptor.Assign(target, value);
								return;
							}
							catch (Exception ex)
							{
								exceptions.Add(ex);

								//try the next
								if (last == descriptor)
									throw new AggregateException(exceptions);
							}
						}
					}
				};
			}
		}
	}
}
