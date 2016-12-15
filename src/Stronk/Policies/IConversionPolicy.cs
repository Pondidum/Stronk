using System;

namespace Stronk.Policies
{
	public interface IConversionPolicy
	{
		void BeforeConversion();
		void OnConversionException(Exception ex);
		void AfterConversion();
	}
}
