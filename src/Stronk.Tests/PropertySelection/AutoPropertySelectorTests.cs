using System.Linq;
using Shouldly;
using Stronk.PropertySelection;
using Xunit;

namespace Stronk.Tests.PropertySelection
{
	public class AutoPropertySelectorTests
	{
		[Fact]
		public void When_selecting_properties_in_a_large_class()
		{
			var selector = new PrivateSetterPropertySelector();
			var properties = selector.Select(typeof(MassiveConfig)).ToArray();

			properties.Count().ShouldBe(200);
		}
	}
}
