using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = new List<string> { "Alice", "Bob", "Charlie" };
        return Ok(users);
    }

    [HttpPost]
            
    
}
