using System.Collections.Generic;
using System.Reflection;

namespace Stronk.PropertyWriters
{
	public class BackingFieldPropertyWriter : IPropertyWriter
	{
		private const BindingFlags PropertyBindingFlags = BindingFlags.Public | BindingFlags.Instance;
		private const BindingFlags FieldBindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase;

		public IEnumerable<PropertyDescriptor> Select(PropertyWriterArgs args)
		{
			var targetType = args.TargetType;
			var properties = targetType.GetProperties(PropertyBindingFlags);
			var descriptors = new List<PropertyDescriptor>(properties.Length);

			foreach (var property in properties)
			{
				var field = targetType.GetField("_" + property.Name, FieldBindingFlags);

				if (field == null)
					field = targetType.GetField(property.Name, FieldBindingFlags);

				if (field != null)
					descriptors.Add(new BackingFieldDescriptor(property.Name, field));

			}

			return descriptors;
		}

		private class BackingFieldDescriptor : PropertyDescriptor
		{
			private readonly FieldInfo _field;

			public BackingFieldDescriptor(string name, FieldInfo field) : base(name, field.FieldType)
			{
				_field = field;
			}

			public override bool IsOptional => base.IsOptional || HasOptionalAttribute(_field.CustomAttributes);

			public override void Assign(object target, object value)
			{
				_field.SetValue(target, value);
			}
		}
	}
}
