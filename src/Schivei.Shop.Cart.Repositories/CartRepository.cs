using Schivei.Shop.Cart.Infrastructure.DTOs.Requests;
using Schivei.Shop.Cart.Infrastructure.DTOs.Responses;
using Schivei.Shop.Cart.Infrastructure.Repositories;
using Schivei.Shop.Cart.Models;

namespace Schivei.Shop.Cart.Repositories;

internal class CartRepository : ICartRepository
{
    public async Task<(CartResponseVM?, Exception?)> AddOrUpdateCartItem(CartId cartId, CartItemUpdateRequestVM request)
    {
        await Task.CompletedTask;

        var cart = Get(cartId) ?? Create(cartId);

        var product = GetSku(request.Sku);

        if (product is null)
            return (null, new("Sku not found"));

        var item = GetCartItem(cart, product.Sku);

        if (item is null)
        {
            cart.Items.Add(new() { Id = product.Id, Sku = product.Sku, Quantity = request.Quantity });
        }
        else
        {
            item.Quantity = request.Quantity;
        }

        return (ToCartResponse(cart), null);
    }

    internal static CartResumeResponseVM ToCartResumeResponse(Models.Cart cart)
    {
        CartResumeResponseVM resume = new()
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Items = cart.Items.Count,
        };

        foreach (var item in cart.Items)
        {
            var sku = GetSku(item.Sku)!;
            resume.Subtotal += item.Quantity * sku.SellingPrice;
        }

        return resume;
    }

    internal static CartResponseVM ToCartResponse(Models.Cart cart)
    {
        var ci = cart.Items.ToArray();
        var items = new CartItemResponseVM[ci.Length];

        for (var i = 0; i < ci.Length; i++)
        {
            var item = ci[i];
            var sku = GetSku(item.Sku)!;
            items[i] = new()
            {
                ProductImage = sku.Product.Thumbnail,
                ProductName = sku.Product.Name,
                ProductPrice = sku.Price,
                ProductSellingPrice = sku.SellingPrice,
                ProductShortDescription = sku.ShortDescription,
                Quantity = item.Quantity,
                SKU = item.Sku
            };
        }

        return new()
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Items = items
        };
    }

    public Task<Exception?> Clear(CartId cartId)
    {
        (Get(cartId) ?? Create(cartId)).Items.Clear();

        return Task.FromResult(null as Exception);
    }

    public async Task<(CartResponseVM?, Exception?)> Create()
    {
        await Task.CompletedTask;

        var cart = Create(Guid.NewGuid());

        return (ToCartResponse(cart), null);
    }

    public async Task<(CartResponseVM?, Exception?)> Create(CreateCartWithItemsRequestVM request)
    {
        await Task.CompletedTask;

        var product = GetSku(request.Sku);

        if (product is null)
            return (null, new InvalidDataException("Sku not found"));

        var cart = Create(Guid.NewGuid());

        cart.UserId = request.UserId;
        cart.Items = new() { new() { Id = product.Id, Quantity = request.Quantity, Sku = request.Sku } };

        DataMocking.Carts.Add(cart);

        return (ToCartResponse(cart), null);
    }

    public async Task<Exception?> DeleteItemBySKU(CartId cartId, string sku)
    {
        await Task.CompletedTask;

        var cart = Get(cartId) ?? Create(cartId);

        var item = cart.Items.FirstOrDefault(x => x.Sku == sku);

        if (item is not null)
            cart.Items.Remove(item);

        return null;
    }

    public async Task<(CartResumeResponseVM?, Exception?)> Resume(CartId cartId)
    {
        await Task.CompletedTask;

        var cart = Get(cartId) ?? Create(cartId);

        return (ToCartResumeResponse(cart!), null);
    }

    public async Task<(CartResponseVM?, Exception?)> OpenCart(CartId cartId)
    {
        await Task.CompletedTask;

        var cart = Get(cartId) ?? Create(cartId);

        return (ToCartResponse(cart), null);
    }

    public async Task<(CartResponseVM?, Exception?)> ReOpen(UserId userId)
    {
        await Task.CompletedTask;

        var cart = GetByUser(userId) ?? Create(Guid.NewGuid(), userId);

        return (ToCartResponse(cart), null);
    }

    public async Task<Exception?> SetUser(CartId cartId, CartSetUserRequestVM request)
    {
        await Task.CompletedTask;

        var cart = Get(cartId) ?? Create(cartId);

        cart.UserId = request.UserId;
        DataMocking.Carts.Add(cart);

        return null;
    }

    private static Models.Cart Create(CartId cartId, UserId userId)
    {
        var cart = new Models.Cart { Id = cartId, UserId = userId };

        DataMocking.Carts.Add(cart);

        return cart;
    }

    private static Models.Cart Create(CartId cartId)
    {
        var cart = new Models.Cart { Id = cartId };

        DataMocking.Carts.Add(cart);

        return cart;
    }

    private static CartItem? GetCartItem(Models.Cart cart, string sku) =>
        cart.Items.FirstOrDefault(x => x.Sku == sku);

    private static ProductVariation? GetSku(string sku) =>
        DataMocking.SKUs.ContainsKey(sku) ? DataMocking.SKUs[sku] : null;

    private static Models.Cart? Get(CartId cartId) =>
        DataMocking.Carts[cartId];

    private static Models.Cart? GetByUser(UserId userId) =>
        DataMocking.Carts[userId];
}
