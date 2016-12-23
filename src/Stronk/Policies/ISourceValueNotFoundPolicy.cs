namespace Stronk.Policies
{
	public interface ISourceValueNotFoundPolicy
	{
		void Handle(SourceValueNotFoundArgs args);
	}
}
