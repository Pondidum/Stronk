using System.Linq;
using Shouldly;
using Stronk.PropertyWriters;
using Xunit;

namespace Stronk.Tests.PropertyWriters
{
	public class BackingFieldPropertyWriterTests
	{
		[Fact]
		public void When_selecing_fields_in_a_large_class()
		{
			var selector = new BackingFieldPropertyWriter();
			var properties = selector
				.Select(new PropertyWriterArgs((message, args) => { }, typeof(MassiveBackingFieldConfig)))
				.ToArray();

			properties.Count().ShouldBe(200);
		}
	}
}
