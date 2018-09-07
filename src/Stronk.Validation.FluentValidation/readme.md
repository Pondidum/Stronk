Use FluentValidation validators with Stronk.

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        var config = new StronkConfig()
            .Validate.With<ConfigurationValidator>()
            .Build<Configuration>();
    }
}

public class Configuration
{
    public TimeSpan Timeout { get; set; }
    public Uri Callback { get; set; }
}

public class ConfigurationValidator : AbstractValidator<Configuration>
{
    private const string[] Hosts = { "localhost", "internal" };

    public ConfigurationValidator()
    {
        var validHosts = new HashSet<string>(Hosts, StringComparer.OrdinalIgnoreCase);

        RuleFor(x => x.Timeout)
            .GreaterThan(TimeSpan.Zero)
            .LessThan(TimeSpan.FromMinutes(2));

        RuleFor(x => x.Callback)
            .Must(url => url.Scheme == Uri.UriSchemeHttps)
            .Must(url => validHosts.Contains(url.Host));
    }
}
```

A complete example can be seen in the [Samples project](https://github.com/Pondidum/Stronk/tree/master/src/Samples/ValidateWithFluentValidation).
