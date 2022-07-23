using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Schivei.Shop.Cart.Infrastructure.DTOs.Requests;
using Schivei.Shop.Cart.Infrastructure.DTOs.Responses;
using Schivei.Shop.Cart.Infrastructure.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Schivei.Shop.Cart.Features;

/// <summary>
/// Shop Cart endpoints
/// </summary>
[ApiController]
[Route("[controller]")]
public partial class CartController : ABaseController
{
    private readonly ICartRepository _cartRepository;

    /// <inheritdoc/>
    public CartController(ICartRepository cartRepository) => _cartRepository = cartRepository;

    /// <summary>
    /// Open a new Cart
    /// </summary>
    [HttpGet]
    [ProducesDefaultResponseType(typeof(CartResponseVM))]
    [ProducesErrorResponseType(typeof(ModelStateDictionary))]
    public Task<IActionResult> Open() =>
        Ok(_cartRepository.Create());

    /// <summary>
    /// Get cart resume
    /// </summary>
    /// <param name="cartId"></param>
    /// <returns></returns>
    [HttpGet("{cartId:Guid}/resume")]
    [ProducesDefaultResponseType(typeof(CartResumeResponseVM))]
    [ProducesErrorResponseType(typeof(ModelStateDictionary))]
    public Task<IActionResult> Resume([FromRoute] Guid cartId) =>
        Ok(_cartRepository.Resume(cartId));

    /// <summary>
    /// Get a cart
    /// </summary>
    /// <param name="cartId"></param>
    [HttpGet("{cartId:Guid}")]
    [ProducesDefaultResponseType(typeof(CartResponseVM))]
    [ProducesErrorResponseType(typeof(ModelStateDictionary))]
    public Task<IActionResult> Get([FromRoute] Guid cartId) =>
        Ok(_cartRepository.OpenCart(cartId));

    /// <summary>
    /// Get a cart by user id
    /// </summary>
    /// <param name="userId">User id</param>
    [HttpGet("user/{userId:Guid}")]
    [ProducesDefaultResponseType(typeof(CartResponseVM))]
    [ProducesErrorResponseType(typeof(ModelStateDictionary))]
    public Task<IActionResult> GetByUser([FromRoute] Guid userId) =>
        Ok(_cartRepository.ReOpen(userId));

    /// <summary>
    /// Defines a user for an opened cart
    /// </summary>
    /// <param name="cartId">Cart Id</param>
    /// <param name="request">Request View Model</param>
    [HttpPatch("{cartId:Guid}/user")]
    [ProducesErrorResponseType(typeof(ModelStateDictionary))]
    public Task<IActionResult> SetUser([FromRoute] Guid cartId, [FromBody] CartSetUserRequestVM request) =>
        Ok(_cartRepository.SetUser(cartId, request));

    /// <summary>
    /// Add an item to cart
    /// </summary>
    /// <param name="cartId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{cartId:Guid}")]
    [ProducesDefaultResponseType(typeof(CartResponseVM))]
    [ProducesErrorResponseType(typeof(ModelStateDictionary))]
    public Task<IActionResult> AddOrUpdateCartItem([FromRoute] Guid cartId, [FromBody] CartItemUpdateRequestVM request) =>
        Ok(_cartRepository.AddOrUpdateCartItem(cartId, request));

    /// <summary>
    /// Create a cart with an item
    /// </summary>
    /// <param name="request"></param>
    [HttpPost]
    [ProducesDefaultResponseType(typeof(CartResponseVM))]
    [ProducesErrorResponseType(typeof(ModelStateDictionary))]
    public Task<IActionResult> CreateCartWithItems([FromBody] CreateCartWithItemsRequestVM request) =>
        Ok(_cartRepository.Create(request));

    /// <summary>
    /// Removes a cart item by sku
    /// </summary>
    /// <param name="cartId">Cart Id</param>
    /// <param name="sku">SKU</param>
    [HttpDelete("{cartId:Guid}/{sku}")]
    [ProducesErrorResponseType(typeof(ModelStateDictionary))]
    public Task<IActionResult> DeleteItem([FromRoute] Guid cartId, [FromRoute, Required(AllowEmptyStrings = false)] string sku) =>
        Ok(_cartRepository.DeleteItemBySKU(cartId, sku));

    /// <summary>
    /// Clear the cart
    /// </summary>
    /// <param name="cartId"></param>
    [HttpDelete("{cartId:Guid}")]
    [ProducesErrorResponseType(typeof(ModelStateDictionary))]
    public Task<IActionResult> ClearCart([FromRoute] Guid cartId) =>
        Ok(_cartRepository.Clear(cartId));
}
