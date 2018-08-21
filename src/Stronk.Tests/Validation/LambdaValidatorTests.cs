using System;
using Shouldly;
using Stronk.Validation;
using Xunit;

namespace Stronk.Tests.Validation
{
	public class LambdaValidatorTests
	{
		[Theory]
		[InlineData(typeof(Target), true)]
		[InlineData(typeof(TargetChild), true)]
		[InlineData(typeof(OtherTarget), false)]
		public void CanValidate_checks_type_properly(Type type, bool works)
		{
			var validator = new LambdaValidator(typeof(Target), target => { });
			var method = validator.GetType().GetMethod(nameof(validator.CanValidate)).MakeGenericMethod(type);

			var result = (bool)method.Invoke(validator, new object[0]);

			result.ShouldBe(works, $"validator.CanValidate<{type.Name}>().ShouldBe({works});");
		}

		[Theory]
		[InlineData(typeof(Target))]
		[InlineData(typeof(TargetChild))]
		public void Validation_works_when_inherited(Type type)
		{
			var wasCalled = false;
			var validator = new LambdaValidator(type, target => wasCalled = true);

			validator.Validate(new TargetChild { Parent = 10 });

			wasCalled.ShouldBe(true);
		}

		private class Target
		{
			public int Parent { get; set; }
		}

		private class TargetChild : Target
		{
			public int Child { get; set; }
		}

		private class OtherTarget
		{
		}
	}
}
