using System.Collections.Generic;
using Stronk.PropertySelection;
using Stronk.ValueConversion;

namespace Stronk.Policies
{
	public interface IConverterNotFoundPolicy
	{
		void Handle(IEnumerable<IValueConverter> availableConverters, PropertyDescriptor property);
	}
}
