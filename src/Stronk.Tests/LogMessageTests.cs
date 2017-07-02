using System;
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
			var message = new LogMessage("A {value} field", new[] { "replaced"});

			message.ToString().ShouldBe("A replaced field");
		}

		[Fact]
		public void When_parsing_a_message_with_multiple_parameters()
		{
			var message = new LogMessage( "Multiple {first} {second} values", new[] { "replaced", "field" });

			message.ToString().ShouldBe("Multiple replaced field values");
		}

		[Fact]
		public void When_parsing_a_message_with_an_object_parameter()
		{
			var value = Guid.NewGuid();
			var message = new LogMessage("An {object} replacement", new object[] { value });

			message.ToString().ShouldBe("An " + value.ToString() + " replacement");
		}
	}
}

