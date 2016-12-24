using System;
using System.Collections.Generic;
using System.Linq;

namespace Stronk.Policies
{
	public class ConversionExceptionPolicy : IConversionExceptionPolicy
	{
		private readonly ConverterExceptionPolicy _policy;
		private readonly List<Exception> _exceptions;

		public ConversionExceptionPolicy(ConverterExceptionPolicy policy)
		{
			_policy = policy;
			_exceptions = new List<Exception>();
		}

		public void BeforeConversion(ConversionExceptionBeforeArgs args)
		{
			_exceptions.Clear();
		}

		public void OnConversionException(ConversionExceptionArgs args)
		{
			if (_policy == ConverterExceptionPolicy.ThrowException)
				throw new ValueConversionException("Error converting", new[] { args.Exception });

			_exceptions.Add(args.Exception);
		}

		public void AfterConversion(ConversionExceptionAfterArgs args)
		{
			if (_exceptions.Any() && _policy == ConverterExceptionPolicy.FallbackOrThrow)
				throw new ValueConversionException("Error converting", _exceptions.ToArray());
		}
	}
}
