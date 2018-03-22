using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Stronk.ValueConverters;
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
				(message, args) => { },
				allConverters.Where(x => x != converter),
				typeof(TTarget),
				sourceValue));
		}


		[Fact]
		public void Single_items_are_convertable() => Convert<T>(_inputSingle).ShouldBe(_expected);

		[Fact]
		public void Arrays_are_convertable() => Convert<T[]>(_inputMultiple).ShouldBe(_expectedCollection);

		[Fact]
		public void ILists_are_convertable() => Convert<IList<T>>(_inputMultiple).ShouldBe(_expectedCollection);

		[Fact]
		public void Lists_are_convertable() => Convert<List<T>>(_inputMultiple).ShouldBe(_expectedCollection);

		[Fact]
		public void IEnumerables_are_convertable() => Convert<IEnumerable<T>>(_inputMultiple).ShouldBe(_expectedCollection);
	}
}
