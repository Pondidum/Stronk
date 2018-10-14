```csharp
public class Startup
{
    public void Configure(IAppBuilder app)
    {
        var config = new StronkConfig()
            //nuget: Stronk.Source.Consul
            .From.Consul(prefix: "ExampleApp/Config")
            .From.AppSettings()
            //nuget: Stronk.Validation.FluentValidation
            .Validate.With<ConfigurationValidator>()
            .Validate.AllSourceValuesAreUsed()
            .Build<Configuration>();    //your configuration poco
    }
}
```

* Read more [documentation here](https://github.com/Pondidum/Stronk)
* View [samples here](https://github.com/Pondidum/Stronk/tree/master/src/Samples)
* Learn about
    * [Configuration Composition](https://andydote.co.uk/2017/11/09/configuration-composition/)
    * [Semantics Configuration Validation](https://andydote.co.uk/2018/08/26/validate-configuration/)
    * [When to Test Your Configuration](https://andydote.co.uk/2018/09/08/semantic-configuration-validation-earlier/)
