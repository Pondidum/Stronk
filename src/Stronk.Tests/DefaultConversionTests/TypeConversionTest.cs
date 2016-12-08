using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Stronk.ValueConversion;
using Xunit;

namespace Stronk.Tests.DefaultConversionTests
{
	public abstract class TypeConversionTest<T>
	{
		private readonly string _inputSingle;
		private readonly string _inputMultiple;
		private readonly T _expected;
		private readonly IEnumerable<T> _expectedCollection;

		public TypeConversionTest(string inputSingle, string inputMultiple, T expected, IEnumerable<T> expectedCollection)
		{
			_inputSingle = inputSingle;
			_inputMultiple = inputMultiple;
			_expected = expected;
			_expectedCollection = expectedCollection;
		}

		private TTarget Convert<TTarget>(string sourceValue)
		{
			var allConverters = Default.ValueConverters;
			var converter = allConverters.First(c => c.CanMap(typeof(TTarget)));

			return (TTarget)converter.Map(new ValueConverterArgs(
				allConverters.Where(x => x != converter),
				typeof(TTarget),
				sourceValue));
		}


		[Fact]
		public void Strings_are_converted() => Convert<T>(_inputSingle).ShouldBe(_expected);

		[Fact]
		public void String_arrays_are_converted() => Convert<T[]>(_inputMultiple).ShouldBe(_expectedCollection);

		[Fact]
		public void String_lists_are_converted() => Convert<IList<T>>(_inputMultiple).ShouldBe(_expectedCollection);

	}
}