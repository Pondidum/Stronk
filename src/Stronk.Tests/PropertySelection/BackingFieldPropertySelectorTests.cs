using System.Linq;
using Shouldly;
using Stronk.PropertySelection;
using Xunit;

namespace Stronk.Tests.PropertySelection
{
	public class BackingFieldPropertySelectorTests
	{
		[Fact]
		public void When_selecing_fields_in_a_large_class()
		{
			var selector = new BackingFieldPropertySelector();
			var properties = selector
				.Select(new PropertySelectorArgs((template, args) => { }, typeof(MassiveBackingFieldConfig)))
				.ToArray();

			properties.Count().ShouldBe(200);
		}
	}
}
