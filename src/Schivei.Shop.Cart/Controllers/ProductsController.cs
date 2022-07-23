using Microsoft.AspNetCore.Mvc;
using Schivei.Shop.Cart.Models;

namespace Schivei.Shop.Cart.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult List() =>
        Ok(DataMocking.Products);
}
