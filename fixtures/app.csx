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