using System.Collections.Generic;
using Stronk.PropertySelection;
using Stronk.ValueConversion;

namespace Stronk.Policies
{
	public class ConverterNotFoundPolicy : IConverterNotFoundPolicy
	{
		private readonly PolicyActions _action;

		public ConverterNotFoundPolicy(PolicyActions action)
		{
			_action = action;
		}

		public void Handle(IEnumerable<IValueConverter> availableConverters, PropertyDescriptor property)
		{
			if (_action == PolicyActions.ThrowException)
				throw new ConverterNotFoundException(availableConverters, property);
		}
	}
}
