using Schivei.Shop.Cart.Infrastructure.DTOs.Requests;
using Schivei.Shop.Cart.Infrastructure.DTOs.Responses;

namespace Schivei.Shop.Cart.Infrastructure.Repositories;

/// <summary>
/// Cart database and services repo
/// </summary>
public interface ICartRepository
{
    /// <summary>
    /// Get cart resume
    /// </summary>
    /// <param name="cartId"></param>
    /// <returns></returns>
    Task<(CartResumeResponseVM?, Exception?)> Resume(Guid cartId);

    /// <summary>
    /// Create new cart
    /// </summary>
    /// <returns></returns>
    Task<(CartResponseVM?, Exception?)> Create();

    /// <summary>
    /// Create a new cart with an item
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<(CartResponseVM?, Exception?)> Create(CreateCartWithItemsRequestVM request);

    /// <summary>
    /// Get cart
    /// </summary>
    /// <param name="cartId"></param>
    /// <returns></returns>
    Task<(CartResponseVM?, Exception?)> OpenCart(Guid cartId);

    /// <summary>
    /// Get Cart by user id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<(CartResponseVM?, Exception?)> ReOpen(Guid userId);

    /// <summary>
    /// Add or update cart item
    /// </summary>
    /// <param name="cartId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<(CartResponseVM?, Exception?)> AddOrUpdateCartItem(Guid cartId, CartItemUpdateRequestVM request);

    /// <summary>
    /// Set user to a cart
    /// </summary>
    /// <param name="cartId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<Exception?> SetUser(Guid cartId, CartSetUserRequestVM request);

    /// <summary>
    /// Clear a cart
    /// </summary>
    /// <param name="cartId"></param>
    /// <returns></returns>
    Task<Exception?> Clear(Guid cartId);

    /// <summary>
    /// Delete cart item by SKU
    /// </summary>
    /// <param name="cartId"></param>
    /// <param name="sku"></param>
    /// <returns></returns>
    Task<Exception?> DeleteItemBySKU(Guid cartId, string sku);
}
