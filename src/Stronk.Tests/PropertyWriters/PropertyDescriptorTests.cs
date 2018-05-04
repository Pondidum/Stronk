using System;
using System.Reflection;
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

		[Optional]
		public string PropertyOne { get; set; }
		public string PropertyTwo { get; set; }
		public bool? PropertyThree { get; set; }

		[Fact]
		public void Propertes_with_an_attribute_starting_with_optional_are_optional()
		{
			var prop = GetType().GetProperty(nameof(PropertyOne));
			var descriptor = new AttributeDescriptor(prop);

			descriptor.IsOptional.ShouldBe(true);
		}

		[Fact]
		public void Propertes_without_an_attribute_starting_with_optional_are_required()
		{
			var prop = GetType().GetProperty(nameof(PropertyTwo));
			var descriptor = new AttributeDescriptor(prop);

			descriptor.IsOptional.ShouldBe(false);
		}

		[Fact]
		public void Propertes_without_an_attribute_but_are_nullable_are_optional()
		{
			var prop = GetType().GetProperty(nameof(PropertyThree));
			var descriptor = new AttributeDescriptor(prop);

			descriptor.IsOptional.ShouldBe(true);
		}

		private class OptionalAttribute : Attribute {}

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

		private class AttributeDescriptor : PropertyDescriptor
		{
			private readonly PropertyInfo _property;

			public AttributeDescriptor(PropertyInfo property) : base(property.Name, property.PropertyType)
			{
				_property = property;
			}

			public override bool IsOptional => base.IsOptional || HasOptionalAttribute(_property.CustomAttributes);

			public override void Assign(object target, object value)
			{
				throw new NotImplementedException();
			}
		}
	}
}
