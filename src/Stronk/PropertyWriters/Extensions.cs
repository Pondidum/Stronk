using Stronk.Dsl;

namespace Stronk.PropertyWriters
{
	public static class Extensions
	{
		public static StronkConfig ToPrivateSetters(this IWriterExpression self)
		{
			return self.To(new PrivateSetterPropertyWriter());
		}

		public static StronkConfig ToBackingFields(this IWriterExpression self)
		{
			return self.To(new BackingFieldPropertyWriter());
		}
	}
}
