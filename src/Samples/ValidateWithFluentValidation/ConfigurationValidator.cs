using System;
using System.Collections.Generic;
using FluentValidation;

namespace ValidateWithFluentValidation
{
	public class ConfigurationValidator : AbstractValidator<Configuration>
	{
		private static readonly HashSet<string> ValidHosts = new HashSet<string>(new[]
		{
			"localhost",
			"internal"
		}, StringComparer.OrdinalIgnoreCase);

		public ConfigurationValidator()
		{
			RuleFor(x => x.Timeout)
				.GreaterThan(TimeSpan.Zero)
				.LessThan(TimeSpan.FromMinutes(2));

			RuleFor(x => x.Callback)
				.Must(url => url.Scheme == Uri.UriSchemeHttps)
				.Must(url => ValidHosts.Contains(url.Host));
		}
	}
}
