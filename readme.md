# Stronk
*Mapping `app.config` an `web.config` to strong typed objects*

## Installation

```powershell
PM> install-package Stronk
```


## Usage

```csharp
public class Startup
{
    public void Configure(IAppBuilder app)
    {
        var config = new StronkConfig()
            .Build<MyApplicationConfiguration>();
        // ...
    }
}

public class MyApplicationConfiguration
{
    public string ApplicationName { get; private set; }
    public int ApiVersion { get; private set; }
    public ConfigurationMode Mode { get: private set; }

    public string MainDB { get; set; }
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

`App.Config` or `Web.Config`
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

## Samples

There are a lot of sample projects in the [src/Samples](https://github.com/Pondidum/Stronk/tree/master/src/Samples) directory for different Stronk use cases (e.g. reading a Json file, reading from Consul, validating a config.)


## Customisation

Stronk provides a DSL to help guide configuration.  By default, it will read from `App.config` or `Web.config`, so you usually only need to write:

```csharp
var config = new StronkConfig()
    .Build<MyApplicationConfiguration>();
```

If you would rather populate an existing object, you can use the `ApplyTo` method instead of `Build`:

```csharp
var config = new MyApplicationConfiguration();

new StronkConfig().ApplyTo(config);
```

You can find all default values used in the [`Default.cs` file](https://github.com/Pondidum/Stronk/blob/master/src/Stronk/Default.cs).

### Configuration Sources

If you want to read from other sources, you can specify them using the `.From` DSL:

```csharp
var config = new StronkConfig()
    .From.EnvironmentVariables()
    .Build<Config>();
```

Optionally, you can specify a prefix to environment variables, which will get stripped off when matching property names in your config (e.g. a prefix of `AppOne:`, will find environment variable called `AppOne:Connection`, and map that to a property called `Connection`):

```csharp
var config = new StronkConfig()
    .From.EnvironmentVariables("SuperAwesomeApp:")
    .Build<Config>();
```

Note that if you specify sources, they will be the only ones used, so if you want to have fallbacks (e.g. read environment variables, but fallback to app.config if one is not available), you need to specify them:

```csharp
var config = new StronkConfig()
    .From.EnvironmentVariables()
    .From.AppConfig()
    .Build<Config>();
```

### Value Conversion

Stronk supports most simple types you will encounter out of the box: `Enums`, `Uri`, `Guid`, `TimeSpan`, `DateTime`, `Nullable<>`, CSV (of any type!), as well as all value types.

Converters are created by implementing `IValueConverter`, or you can use the `LambdaValueConverter<T>` if you need something simple (for example, `Guid` conversion is defined as `new LambdaValueConverter<Guid>(Guid.Parse)`).

You can either add additional value converters to what Stronk can use by default:

```csharp
var config = new StronkConfig()
    .Convert.Using(new LambdaValueConverter<CustomThing>(val => CustomThing.Parse(val)))
    .Build<Config>();
```

Or replace all default converters with your own (not recommended!):

```csharp
var config = new StronkConfig()
    .Convert.UsingOnly(new LambdaValueConverter<CustomThing>(val => CustomThing.Parse(val)))
    .Build<Config>();
```

### Property Mapping

By default, Stronk will pick from values in your configuration sources where the key matches the property name (case insensitive).  If you want to replace this behaviour, you can implement a custom `IPropertyMapper`:

```csharp
public class PropertyNamePropertyMapper : IPropertyMapper
{
    public string ValueFor(PropertyMapperArgs args) => args.GetValue(args.Property.Name);
}
```

```csharp
var config = new StronkConfig()
    .Map.With(new PropertyNamePropertyMapper())
    .Build<Config>();
```

### Property Writing

By default, Stronk can write to properties with a setter (no matter its visibility), and to backing fields for properties, when the backing field is the same, but with the `_` prefix (again, case insensitive).  It prefers the properties with setters.

You can override this using the `.Write` DSL:

```csharp
var config = new StronkConfig()
    .Write.To(new BackingFieldPropertyWriter())
    .Build<Config>();
```

If Stronk cannot find a value for a property, it will throw a `SourceValueNotFoundException`.  If you do need an optional property, you do one of the following:

Make the type of the property Nullable:

```csharp
public class Configuration
{
    public bool? IsLive { get; private set; }
}
```

Mark the property as optional with an attribute whose name starts with `Optional`:

```csharp
public class Configuration
{
    [Optional]
    public string IsLive { get; private set; }
}

public class OptionalAttribute : Attribute {}
```

### Validation

It's always a good idea to make sure your configuration is not only validly loaded, but also that the values make semantic sense.  For example, if you have a `Timeout` property being mapped to a `TimeSpan`, not only should the value be formatted correctly, but there is a probably a minimum and maximum value which would make sense too.

```csharp
var config = new StronkConfig()
    .Validate.Using<Configuration>(c =>
    {
        if (c.Timeout < TimeSpan.FromSeconds(60) && c.Timeout > TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(c.Timeout), $"Must be greater than 0, and less than 1 minute");
    });
    .Build<Config>();
```

You can of course use other libraries (such as [FluentValidation](https://github.com/JeremySkinner/FluentValidation)) to perform the actual validation, in which case your configuration might look like this:

```csharp
var config = new StronkConfig()
    .Validate.Using<Configuration>(c => new ConfigValidator().ValidateAndThrow(c))
    .Build<Config>();
```

### Logging

Want to know what Stronk did while populating your object? You can specify a logger to use with the `.Log` DSL:

```csharp
var config = new StronkConfig()
    .Log.Using(message => Log.Debug(message.Template, message.Args))
    .Build<Config>();
```

The log messages are structured - so you can use them directly with [Serilog](https://serilog.net/) or similar libraries.  If your logging library is not structured, just call `.ToString()` on the message object, and you will get a flat string, with all that useful structure gone.

## Questions

### Why not use Microsoft.Configuration?

That didn't exist when I wrote this library.  Also, support for non dotnet core is somewhat lacking (e.g. reading a `web.config` in XML).

### Will you support dotnet core

Undecided.<br />
I could move the only dependency on `ConfigurationManager` to a separate package and then target core...but if you're on core, you might as well use `Microsoft.Configuration`.

### How about Json configuration files?

I might add this as a separate package in the future (e.g. `Stronk.Sources.Json` or similar), but I don't want (any) dependencies on other libraries from the main Stronk library.

### How about deserializing json inside a value?

Implement a custom `IValueConverter`.  There is a [sample of this here](https://github.com/Pondidum/Stronk/blob/master/src/Samples/ReadFromJsonFile/JsonConfigFile.cs).

### I have other questions

Cool!  Either open an issue on this repo or feel free to tweet me ([@pondidum](https://twitter.com/Pondidum)) :)
