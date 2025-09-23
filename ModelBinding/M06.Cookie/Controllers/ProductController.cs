
using Microsoft.AspNetCore.Mvc;

namespace M06.Body.Controllers;

[ApiController]
public class ProductController: ControllerBase
{
  [HttpGet("cookie-controller")]
  public IActionResult Get()
  {
        return Ok(HttpContext.Request.Cookies["session-id"]);
  }
}

