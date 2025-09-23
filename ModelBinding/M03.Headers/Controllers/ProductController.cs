using Microsoft.AspNetCore.Mvc;

namespace M03.Headers.Controllers;

[ApiController]
public class ProductController: ControllerBase
{
    [HttpGet("product-controller")]
    public IActionResult Get([FromHeader(Name = "X-Api-Version")] string apiVersion)
    { 
        return Ok($"Api version: {apiVersion}");
    }
}

