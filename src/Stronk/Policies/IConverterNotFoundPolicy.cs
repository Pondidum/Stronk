namespace Stronk.Policies
{
	public interface IConverterNotFoundPolicy
	{
		void Handle(ConverterNotFoundArgs args);
	}
}
