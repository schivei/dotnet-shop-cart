using Microsoft.AspNetCore.Mvc;
using Schivei.Shop.Cart.Models;
using System.Diagnostics.CodeAnalysis;

namespace Schivei.Shop.Cart.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class ResetController : ControllerBase
    {
        [HttpGet]
        public IActionResult Reset()
        {
            DataMocking.Products.SelectMany(p => p.Variations)
                .AsParallel().ForAll(p => p.Stock = 10);

            DataMocking.Carts.Clear();

            return Ok();
        }
    }
}
