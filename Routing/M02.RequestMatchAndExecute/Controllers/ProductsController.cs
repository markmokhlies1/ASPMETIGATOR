using Microsoft.AspNetCore.Mvc;

namespace M02.RequestMatchAndExecute.Controllers;


[ApiController]
[Route("[controller]")] // ../products
public class ProductsController: ControllerBase
{
    // ../products/all
    [HttpGet("all")]
    public IActionResult GetProducts()
    {
        return Ok(new []{
            "Product #1",
            "Product #2"
        });
    }
}
