using System;
using System.Linq;
using Shouldly;
using Xunit;

namespace Stronk.Tests
{
	public class LogMessageTests
	{
		[Fact]
		public void When_parsing_a_message_with_no_parameters()
		{
			var message = new LogMessage("Nothing special", new object[0]);

			message.ToString().ShouldBe("Nothing special");
		}

		[Fact]
		public void When_parsing_a_message_with_one_parameter()
		{
			var message = new LogMessage("A {value} field", new[] { "replaced" });

			message.ToString().ShouldBe("A replaced field");
		}

		[Fact]
		public void When_parsing_a_message_with_multiple_parameters()
		{
			var message = new LogMessage("Multiple {first} {second} values", new[] { "replaced", "field" });

			message.ToString().ShouldBe("Multiple replaced field values");
		}

		[Fact]
		public void When_parsing_a_message_with_an_object_parameter()
		{
			var value = Guid.NewGuid();
			var message = new LogMessage("An {object} replacement", new object[] { value });

			message.ToString().ShouldBe("An " + value.ToString() + " replacement");
		}

		[Fact]
		public void When_a_parameter_is_an_array_it_gets_turned_into_a_csv()
		{
			var value = new[] { "one", "two", "three" };
			var message = new LogMessage("A Csv: {here}.", new object[] { value });

			message.ToString().ShouldBe("A Csv: one, two, three.");
		}

		[Fact]
		public void When_a_parameter_is_a_linq_iterator_it_gets_turned_into_a_csv()
		{
			var value = Enumerable.Range(1, 3).Select(i => i.ToString());
			var message = new LogMessage("A Csv: {here}.", new object[] { value });

			message.ToString().ShouldBe("A Csv: 1, 2, 3.");
		}
	}
}
