# Stronk
*Mapping `app.config` an `web.config` to strong typed objects*

## Usage:

```csharp
public class MyApplicationConfiguration
{
    public string ApplicationName { get; private set; }
    public int ApiVersion { get; private set; }
    public ConfigurationMode Mode { get: private set; }

    public string MainDB { get; set; }

    public MyApplicationConfiguration()
    {
        this.FromAppConfig();

        // this is identical to FromAppConfig, it's here to make you happy if you are reading a web.config
        //this.FromWebConfig():

        // this is an idea for .net core. Subject to change. a lot.
        //this.FromConfigurationProviderThingy(itGoesHere.Build());
    }
}

public enum ConfigurationMode
{
    Local,
    Dev,
    Test,
    QA,
    ExternalTest,
    Production
}
```

```xml
<configuration>
  <appSettings>
    <add key="ApplicationName" value="testing" />
    <add key="ApiVersion" value="12" />
    <add key="Mode" value="QA" />
  </appSettings>
  <connectionStrings>
    <add name="MainDb" connectionString="Some Connection String Here" />
  </connectionStrings>
</configuration>
```

## Customisation

```csharp
public class PrivatePropertyBindingPolicy : IBindingPolicy
{
    public Action<object, object> CreateBinder(BindingPolicyArgs args)
    {
        var target = args.TargetType; // typeof(SomeApplication.MyApplicationConfiguration)
        var name = args.SourceName; //"ApiVersion"

        var setter = target
          .GetProperties(BindingFlags.Instance | BindingFlags.Public)
          .Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
          .Where(p => p.CanWrite)
          .Select(p => p.GetSetMethod(nonPublic: true))
          .SingleOrDefault();

        if (setter == null)
            return null;

        return (config, value) => setter.Invoke(config, new[] { value });
    }
}
```

```csharp
public static void Main()
{
    StronkConfiguration.Policies(c => {
      c.Use(new ConnectionStringSuffixMappingPolicy("DB"));
      c.Use(new PrivatePropertyBindingPolicy());
      c.Use(new BackingFieldBindingPolicy());
    })
}
```
