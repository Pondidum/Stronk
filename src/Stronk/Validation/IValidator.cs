namespace Stronk.Validation
{
	public interface IValidator
	{
		bool CanValidate<T>();
		void Validate<T>(T target);
	}
}
