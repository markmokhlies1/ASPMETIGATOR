using M01.ActionFilters.Filters;
using Microsoft.AspNetCore.Mvc;

namespace M01.FiltersController.Controllers;

[ApiController]
[Route("api/products")]
// [TrackActionTimeFilterV2]  // Controller level Registration
public class ProductController() : ControllerBase
{

    [HttpGet]
    [TrackActionTimeFilterV3]
    public IActionResult Get()
    {
        return Ok(new[] { "Keyboard [$52.99]", "Mouse, [$34.99]" });
    }
}

