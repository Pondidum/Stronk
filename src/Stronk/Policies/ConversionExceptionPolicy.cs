using System;
using System.Collections.Generic;
using System.Linq;
using Stronk.PropertyWriters;

namespace Stronk.Policies
{
	public class ConversionExceptionPolicy
	{
		private readonly PropertyDescriptor _property;
		private readonly string _value;
		private readonly List<Exception> _exceptions;

		public ConversionExceptionPolicy(PropertyDescriptor property, string value)
		{
			_property = property;
			_value = value;
			_exceptions = new List<Exception>();
		}

		public void OnConversionException(Exception ex) => _exceptions.Add(ex);

		public void AfterConversion()
		{
			if (_exceptions.Any())
				throw new ValueConversionException(BuildMessage(), _exceptions.ToArray());
		}

		private string BuildMessage()
			=> $"Error converting the value '{_value}' to type '{_property.Type.Name}' for property '{_property.Name}'";
	}
}
