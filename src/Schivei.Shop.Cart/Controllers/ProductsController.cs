using Microsoft.AspNetCore.Mvc;
using Schivei.Shop.Cart.Models;
using System.Diagnostics.CodeAnalysis;

namespace Schivei.Shop.Cart.Controllers;

[ApiController]
[Route("[controller]")]
[ExcludeFromCodeCoverage]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult List() =>
        Ok(DataMocking.Products);
}
