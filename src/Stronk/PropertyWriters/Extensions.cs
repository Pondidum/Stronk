using Stronk.Dsl;

namespace Stronk.PropertyWriters
{
	public static class Extensions
	{
		public static StronkConfig ToPrivateSetters(this WriterExpression self)
		{
			return self.To(new PrivateSetterPropertyWriter());
		}

		public static StronkConfig ToBackingFields(this WriterExpression self)
		{
			return self.To(new BackingFieldPropertyWriter());
		}
	}
}
