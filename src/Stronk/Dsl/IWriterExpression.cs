using Stronk.PropertyWriters;

namespace Stronk.Dsl
{
	public interface IWriterExpression
	{
		StronkConfig To(IPropertyWriter writer);
	}
}