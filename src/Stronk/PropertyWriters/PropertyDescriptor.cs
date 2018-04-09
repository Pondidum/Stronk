using System;

namespace Stronk.PropertyWriters
{
	public abstract class PropertyDescriptor
	{
		public string Name { get; }
		public Type Type { get; }

		public PropertyDescriptor(string name, Type type)
		{
			Name = name;
			Type = type;
		}

		public abstract void Assign(object target, object value);
	}
}
