using System;
using System.Collections.Generic;
using System.Linq;

namespace Stronk.Policies
{
	public class ConversionPolicy : IConversionPolicy
	{
		private readonly ConverterExceptionPolicy _policy;
		private readonly List<Exception> _exceptions;

		public ConversionPolicy(ConverterExceptionPolicy policy)
		{
			_policy = policy;
			_exceptions = new List<Exception>();
		}

		public void BeforeConversion(ConversionPolicyBeforeArgs args)
		{
			_exceptions.Clear();
		}

		public void OnConversionException(ConversionPolicyExceptionArgs args)
		{
			if (_policy == ConverterExceptionPolicy.ThrowException)
				throw new ValueConversionException("Error converting", new[] { args.Exception });

			_exceptions.Add(args.Exception);
		}

		public void AfterConversion(ConversionPolicyAfterArgs args)
		{
			if (_exceptions.Any() && _policy == ConverterExceptionPolicy.FallbackOrThrow)
				throw new ValueConversionException("Error converting", _exceptions.ToArray());
		}
	}
}
