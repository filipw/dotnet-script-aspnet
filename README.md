# Experimental playground for Scripting ASP.NET Core apps

TL;DR

1) Create a project.json with your dependencies

2) Create a script file (CSX) where you can interact with ASP.NET core pipeline, set up services, configure MVC, add controllers etc. For example:

```
[Route("api/[controller]")]
public class ValuesController : Controller
{
    // GET api/values
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }
}

Services(s => s.AddMvc());
Configure((app, env) => app.UseMvc());
```

3) Run `dotnet aspnet-script {script_name}.csx`. This will launch Kestrel.

4) Profit.
