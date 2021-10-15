using Microsoft.AspNetCore.Mvc;

namespace MyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class MeController : ControllerBase
{

    [HttpGet]
    public Me Get()
    {
        var me = new Me(){ 
            Name = "Northon",
            Age = 30,
            BirthDate = new DateTime(1991,1,14)
        };

        return me;
    }
}
