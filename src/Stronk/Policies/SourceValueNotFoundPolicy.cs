namespace Stronk.Policies
{
	public class SourceValueNotFoundPolicy : ISourceValueNotFoundPolicy
	{
		private readonly PolicyActions _action;

		public SourceValueNotFoundPolicy(PolicyActions action)
		{
			_action = action;
		}

		public void Handle(SourceValueNotFoundArgs args)
		{
			if (_action == PolicyActions.ThrowException)
				throw new SourceValueNotFoundException(args.ValueSelectors, args.Property);
		}
	}
}
