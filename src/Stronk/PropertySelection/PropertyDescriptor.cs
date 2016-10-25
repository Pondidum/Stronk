using System;

namespace Stronk.PropertySelection
{
	public class PropertyDescriptor
	{
		public string Name { get; set; }
		public Type Type { get; set; }
		public Action<object, object> Assign { get; set; }
	}
}
