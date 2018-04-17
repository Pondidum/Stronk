using System;

namespace Stronk.PropertyWriters
{
	public abstract class PropertyDescriptor
	{
		public string Name { get; }
		public Type Type { get; }
		public bool IsOptional { get; }

		public PropertyDescriptor(string name, Type type)
		{
			Name = name;
			Type = type;
			IsOptional = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		public abstract void Assign(object target, object value);
	}
}
