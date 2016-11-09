# Experimental playground for Scripting ASP.NET Core apps

## What is this? 

Quickly hacked together at MVP Summit. The idea is, you can write an ASP.NET Core app as a script, with lowest possible ceremony, and then run them using .NET CLI.

It's based on Roslyn scripting and my [dotnet-script](https://github.com/filipw/dotnet-script) project.

## Getting up and running

1) Create a `project.json` with your dependencies. You can reference anything that works on .NET Core. `Microsoft.AspNetCore.Server.Kestrel` and `Microsoft.AspNetCore.Mvc` are implicitly available to you. You also need to reference the `Dotnet.Script.AspNet` as a tool - as we will be using it for running the app.

Sample `project.json`

```
{
  "frameworks": {
    "netcoreapp1.0": {
      "dependencies": {
        "GenFu": "1.2.1"
      }
    }
  },
  "tools": {
    "Dotnet.Script.AspNet": {
      "version": "0.0.1-beta",
      "imports": [
        "portable-net45+win8",
        "dnxcore50"
      ]
    }
  }
}
```

In the above case, `GenFu` is a package we wanna use in our ASP.NET Core app and `Dotnet.Script.AspNet` is mandatory (it's a runner). Again, no need to reference `Microsoft.AspNetCore.Server.Kestrel` and `Microsoft.AspNetCore.Mvc` - but you will need to pull in any other package you want.

2) `dotnet restore`

3) Create a script file (CSX) where you can interact with ASP.NET core pipeline, set up services, configure MVC, add controllers, models, use NuGet packages etc. The hooks from the `Startup` class are surfaced as `Confgure` and `Services` global methods. Sample is shown below.

```
using GenFu;

[Route("api/[controller]")]
public class PersonController : Controller
{
    [HttpGet]
    public Person Get()
    {
        var person = A.New<Person>();
        return person;
    }
}

public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

Services(s => s.AddMvc());
Configure((app, env) => app.UseMvc());
```

3) Run `dotnet script-aspnet {script_name}.csx`. This will launch Kestrel and start your app. 

4) Profit.

![](http://g.recordit.co/rbJvrQe47u.gif)
