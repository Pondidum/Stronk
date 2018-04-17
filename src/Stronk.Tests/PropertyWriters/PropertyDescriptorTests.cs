using System;
using Shouldly;
using Stronk.PropertyWriters;
using Xunit;

namespace Stronk.Tests.PropertyWriters
{
	public class PropertyDescriptorTests
	{
		[Theory]
		[InlineData(typeof(int), false)]
		[InlineData(typeof(int?), true)]
		public void Should_detect_optional_types(Type type, bool expected)
		{
			var property = new TestDescriptor("Test", type);

			if (expected)
				property.IsOptional.ShouldBeTrue();
			else
				property.IsOptional.ShouldBeFalse();
		}

		private class TestDescriptor : PropertyDescriptor
		{
			public TestDescriptor(string name, Type type) : base(name, type)
			{
			}

			public override void Assign(object target, object value)
			{
				throw new NotImplementedException();
			}
		}
	}
}
