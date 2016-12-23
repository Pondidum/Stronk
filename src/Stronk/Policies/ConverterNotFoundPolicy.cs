namespace Stronk.Policies
{
	public class ConverterNotFoundPolicy : IConverterNotFoundPolicy
	{
		private readonly PolicyActions _action;

		public ConverterNotFoundPolicy(PolicyActions action)
		{
			_action = action;
		}

		public void Handle(ConverterNotFoundArgs args)
		{
			if (_action == PolicyActions.ThrowException)
				throw new ConverterNotFoundException(args.AvailableConverters, args.Property);
		}
	}
}
