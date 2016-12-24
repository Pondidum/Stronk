using System;

namespace Stronk.Policies
{
	public interface IConversionPolicy
	{
		void BeforeConversion(ConversionPolicyBeforeArgs args);
		void OnConversionException(ConversionPolicyExceptionArgs args);
		void AfterConversion(ConversionPolicyAfterArgs args);
	}

	public class ConversionPolicyBeforeArgs
	{
	}

	public class ConversionPolicyAfterArgs
	{
	}

	public class ConversionPolicyExceptionArgs
	{
		public Exception Exception { get; set; }
	}
}
