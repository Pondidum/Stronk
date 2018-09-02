using System;
using System.Collections.Generic;
using FluentValidation;
using Shouldly;
using Stronk.ConfigurationSources;
using Xunit;

namespace Stronk.Validation.FluentValidation.Tests
{
	public class ValidationTests
	{
		private readonly StronkConfig _builder;

		public ValidationTests()
		{
			var source = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			source[nameof(TargetParent.ParentValue)] = "1";
			source[nameof(Target.TargetValue)] = "2";
			source[nameof(TargetChild.ChildValue)] = "3";

			_builder = new StronkConfig()
				.From.Source(new DictionarySource(source))
				.Validate.Using<TargetValidator>();
		}

		[Fact]
		public void When_validating_an_exact_type()
		{
			Should.Throw<ValidationException>(() => _builder.Build<Target>());
		}

		[Fact]
		public void When_validating_a_child_type()
		{
			Should.Throw<ValidationException>(() => _builder.Build<TargetChild>());
		}

		[Fact]
		public void When_validating_a_parent_type()
		{
			Should.Throw<InvalidOperationException>(() => _builder.Build<TargetParent>());
		}

		[Fact]
		public void When_validating_a_different_type()
		{
			Should.Throw<InvalidOperationException>(() => _builder.Build<Other>());
		}

		public class TargetParent
		{
			public int ParentValue { get; set; }
		}

		public class Target : TargetParent
		{
			public int TargetValue { get; set; }
		}

		public class TargetChild : Target
		{
			public int ChildValue { get; set; }
		}

		public class Other
		{
		}

		public class TargetValidator : AbstractValidator<Target>
		{
			public TargetValidator()
			{
				RuleFor(x => x.ParentValue).GreaterThan(10);
				RuleFor(x => x.TargetValue).GreaterThan(10);
			}
		}
	}
}
