using System.Linq;
using Shouldly;
using Stronk.PropertyWriters;
using Xunit;

namespace Stronk.Tests.PropertyWriters
{
	public class AutoPropertyWriterTests
	{
		[Fact]
		public void When_selecting_properties_in_a_large_class()
		{
			var selector = new PrivateSetterPropertyWriter();
			var properties = selector
				.Select(new PropertyWriterArgs(message => {}, typeof(MassiveConfig)))
				.ToArray();

			properties.Count().ShouldBe(200);
		}
	}
}
