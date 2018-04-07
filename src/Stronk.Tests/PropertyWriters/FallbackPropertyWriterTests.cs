using System;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Stronk.PropertyWriters;
using Xunit;

namespace Stronk.Tests.PropertyWriters
{
	public class FallbackPropertyWriterTests
	{
		private readonly string _value;
		private readonly string _otherValue;
		private readonly Target _target;

		public FallbackPropertyWriterTests()
		{
			_target = new Target();
			_value = Guid.NewGuid().ToString();
			_otherValue = Guid.NewGuid().ToString();
		}

		private void Assign(params IPropertyWriter[] writers) =>
			new FallbackPropertyWriter(writers)
				.Select(new PropertyWriterArgs((message, args) => { }, typeof(Target)))
				.Single()
				.Assign(_target, _value);

		[Fact]
		public void When_there_is_one_writer_which_works()
		{
			Assign(new WorkingWriter(_value));

			_target.Value.ShouldBe(_value);
		}

		[Fact]
		public void When_there_is_one_writer_which_throws()
		{
			var ex = Should.Throw<AggregateException>(() => Assign(new ThrowingWriter(new ExpectedException())));

			ex.InnerExceptions
				.ShouldHaveSingleItem()
				.ShouldBeOfType<ExpectedException>();

			_target.Value.ShouldBeNull();
		}

		[Fact]
		public void When_there_are_two_writers_and_both_work()
		{
			Assign(
				new WorkingWriter(_value),
				new WorkingWriter(_otherValue)
			);

			_target.Value.ShouldBe(_value);
		}

		[Fact]
		public void When_there_are_two_writers_and_first_throws()
		{
			Assign(
				new ThrowingWriter(new UnExpectedException()),
				new WorkingWriter(_value)
			);

			_target.Value.ShouldBe(_value);
		}

		[Fact]
		public void When_there_are_two_writers_and_the_second_throws()
		{
			Assign(
				new WorkingWriter(_value),
				new ThrowingWriter(new UnExpectedException())
			);

			_target.Value.ShouldBe(_value);
		}

		[Fact]
		public void When_there_are_two_writers_and_both_throw()
		{
			var ex = Should.Throw<AggregateException>(() => Assign(
				new ThrowingWriter(new ExpectedException()),
				new ThrowingWriter(new ExpectedException())
			));

			ex.ShouldSatisfyAllConditions(
				() => ex.InnerExceptions.Count.ShouldBe(2),
				() => ex.InnerExceptions.First().ShouldBeOfType<ExpectedException>(),
				() => ex.InnerExceptions.Last().ShouldBeOfType<ExpectedException>()
			);
			_target.Value.ShouldBeNull();
		}


		private class Target
		{
			public string Value { get; set; }
		}

		public class WorkingWriter : IPropertyWriter
		{
			private readonly string _value;

			public WorkingWriter(string value) => _value = value;

			public IEnumerable<PropertyDescriptor> Select(PropertyWriterArgs args)
			{
				yield return new PropertyDescriptor
				{
					Name = nameof(Target.Value),
					Type = typeof(string),
					Assign = (t, v) => ((Target)t).Value = _value
				};
			}
		}

		public class ThrowingWriter : IPropertyWriter
		{
			private readonly Exception _exception;

			public ThrowingWriter(Exception exception)
			{
				_exception = exception;
			}

			public IEnumerable<PropertyDescriptor> Select(PropertyWriterArgs args)
			{
				yield return new PropertyDescriptor
				{
					Name = nameof(Target.Value),
					Type = typeof(string),
					Assign = (t, v) => throw _exception
				};
			}
		}

		private class ExpectedException : Exception
		{
			public ExpectedException() : base("This was supposed to be thrown")
			{
			}
		}

		private class UnExpectedException : Exception
		{
			public UnExpectedException() : base("This was not supposed to be thrown")
			{
			}
		}
	}
}
