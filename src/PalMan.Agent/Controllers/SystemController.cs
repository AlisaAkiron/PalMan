using Microsoft.AspNetCore.Mvc;

namespace PalMan.Agent.Controllers;

[ApiController]
[Route("/v1")]
public class SystemController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("pong");
    }
}
