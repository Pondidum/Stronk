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

## Customisation

You can configure **how** Stronk handles configuration, and **where** Stronk reads it from.

The rough pipeline is as follows:

Select Properties to be populated
-> Get a Value from `ConfigurationSource` for each Property
-> Find an `IValueConverter` for the property
-> Convert the value with the selected converter
-> Assign to the property

To specify your own conversion and mapping you can either implement `IStronkOptions`, or use the customisation methods on `StronkOptions`, e.g.

```csharp
var sc = new StronkOptions();
sc.AddBefore<EnumValueConverter>(new SpecialEnumValueConverter());

this.FromAppConfig(sc);
```

### Customising Conversion and Mapping

#### Property Selection
*This is used to scan the target `Type` and provide a set of `PropertyDescriptor`s for it.*

Stronk comes with two implementations of `IPropertySelector`, which are both enabled by default:

* [PrivateSetterPropertySelector](https://github.com/Pondidum/Stronk/blob/master/src/Stronk/PropertySelection/PrivateSetterPropertySelector.cs) - this will select any public property which can be written to.
* [BackingFieldPropertySelector](https://github.com/Pondidum/Stronk/blob/master/src/Stronk/PropertySelection/BackingFieldPropertySelector.cs)) - this will select any public property with a backing field which matches the property name (optionally with a preceding underscore.)

In your own implementation, you just need to return an enumerable of `PropertyDescriptor`:

```csharp
new PropertyDescriptor
{
	Name = prop.Name,
	Type = prop.PropertyType,
	Assign = (target, value) => prop.GetSetMethod(true).Invoke(target, new[] { value })
}
```

#### Value Selection
*This is used to match a `PropertyDescriptor` to a value provided by `IConfigurationSource`.*

Stronk comes with one implementation of `ISourceValueSelector`, which is enabled by default:

* [PropertyNameSourceValueSelector](https://github.com/Pondidum/Stronk/blob/master/src/Stronk/ValueSelection/PropertyNameSourceValueSelector.cs) - this will return a value from the `AppSettings` section of the app.config file, matching on `PropertyDescriptor.Name`, if there is no match in `AppSettings`, it will try the `connectionStrings` section also.

#### Value Conversion
*This is used to take the value from an `ISourceValueSelector` and convert it to the type from `PropertyDescriptor`.*

Stronk comes with many converters, which are attempted to be used in order of specification.  By default this is the conversion order:
* Uri - calls `val, type => new Uri(val)`
* Guid - calls `val, type => Guid.Parse(val)`
* [EnumValueConverter](https://github.com/Pondidum/Stronk/blob/master/src/Stronk/ValueConversion/EnumValueConverter.cs) - Makes use of `Enum.Parse` and `Enum.IsDefined`
* [CsvValueConverter](https://github.com/Pondidum/Stronk/blob/master/src/Stronk/ValueConversion/CsvValueConverter.cs) - calls other value converters to convert individual values
* [FallbackValueConverter](https://github.com/Pondidum/Stronk/blob/master/src/Stronk/ValueConversion/FallbackValueConverter.cs) - calls `val, type => Convert.ChangeType(val, type)`

The easiest way of creating a new converter is to just add an instance of `LambdaValueConverter<T>` to the `IStronkOptions`.  This is how `Uri` and `Guid` are implemented.

### Customising Configuration Source
*This is used to customise where Stronk will read configuration values from.*

Stronk comes with one implementaion of `IConfigurationSource`:

* [AppConfigSource](https://github.com/Pondidum/Stronk/blob/master/src/Stronk/AppConfigSource.cs) - this uses the `ConfigurationManager` class, which means both `App.Config` and `Web.Config` are supported out of the box.

## To Do

* Customisable error policy
  * no value found
  * no converter found
  * converter was not happy
