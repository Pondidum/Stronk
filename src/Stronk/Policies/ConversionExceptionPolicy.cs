using System;
using System.Collections.Generic;
using System.Linq;

namespace Stronk.Policies
{
	public class ConversionExceptionPolicy : IConversionExceptionPolicy
	{
		private readonly ConverterExceptionPolicy _policy;
		private readonly List<ConversionExceptionArgs> _exceptions;

		public ConversionExceptionPolicy(ConverterExceptionPolicy policy)
		{
			_policy = policy;
			_exceptions = new List<ConversionExceptionArgs>();
		}

		public void BeforeConversion(ConversionExceptionBeforeArgs args)
		{
			_exceptions.Clear();
		}

		public void OnConversionException(ConversionExceptionArgs args)
		{
			if (_policy == ConverterExceptionPolicy.ThrowException)
				throw new ValueConversionException(BuildMessage(args), new[] { args.Exception });

			_exceptions.Add(args);
		}

		public void AfterConversion(ConversionExceptionAfterArgs args)
		{
			if (!_exceptions.Any() || _policy != ConverterExceptionPolicy.FallbackOrThrow)
				return;

			throw new ValueConversionException(
				BuildMessage(_exceptions.First()),
				_exceptions.Select(e => e.Exception).ToArray());
		}

		private static string BuildMessage(ConversionExceptionArgs args)
			=> $"Error converting the value '{args.Value}' to type '{args.Property.Type.Name}' for property '{args.Property.Name}'";
	}
}
