using System.Linq;

namespace Stronk.Validation
{
	public class ValueSelectorValidator : IValidator
	{
		public bool CanValidate<T>() => typeof(T) == typeof(ValueSelector);

		public void Validate<T>(T target)
		{
			var selector = target as ValueSelector;

			if (selector == null)
				return;

			var unusedKeys = selector.GetUnusedKeys();
			if (unusedKeys.Any())
				throw new UnusedConfigurationEntriesException(unusedKeys);
		}
	}
}
