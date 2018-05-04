using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Stronk.PropertyWriters
{
	public abstract class PropertyDescriptor
	{
		public string Name { get; }
		public Type Type { get; }
		public virtual bool IsOptional => IsTypeOptional(Type);

		public PropertyDescriptor(string name, Type type)
		{
			Name = name;
			Type = type;
		}

		public abstract void Assign(object target, object value);

		protected static bool IsTypeOptional(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		protected static bool HasOptionalAttribute(IEnumerable<CustomAttributeData> attributes) => attributes.Any(a => a.AttributeType.Name.StartsWith("Optional", StringComparison.OrdinalIgnoreCase));
	}
}
