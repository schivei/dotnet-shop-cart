using Schivei.Shop.Cart.Infrastructure.DTOs.Requests;
using Schivei.Shop.Cart.Infrastructure.DTOs.Responses;
using Schivei.Shop.Cart.Infrastructure.Repositories;
using Schivei.Shop.Cart.Models;

namespace Schivei.Shop.Cart.Repositories;

internal class CartRepository : ICartRepository
{
    public async Task<(CartResponseVM?, Exception?)> AddOrUpdateCartItem(Guid cartId, CartItemUpdateRequestVM request)
    {
        await Task.CompletedTask;

        var cart = DataMocking.Carts.FirstOrDefault(c => c.Id == cartId);

        if (cart is null)
            return (null, null);

        var product = DataMocking.Products.SelectMany(p => p.Variations)
            .SingleOrDefault(p => p.Sku == request.Sku);

        if (product is null)
            return (null, new InvalidDataException("Sku not found"));

        var item = cart.Items.FirstOrDefault(i => i.Sku == request.Sku);

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
        var skus = DataMocking.Products.SelectMany(p => p.Variations)
            .ToDictionary(x => x.Sku, x => x);

        CartResumeResponseVM resume = new()
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Items = cart.Items.Count,
        };

        foreach (var item in cart.Items)
            resume.Subtotal += item.Quantity * skus[item.Sku].SellingPrice;

        return resume;
    }

    internal static CartResponseVM ToCartResponse(Models.Cart cart)
    {
        var skus = DataMocking.Products.SelectMany(p => p.Variations)
            .ToDictionary(x => x.Sku, x => x);

        return new()
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Items = cart.Items
            .Select(i => new CartItemResponseVM
            {
                ProductImage = skus[i.Sku].Product.Thumbnail,
                ProductName = skus[i.Sku].Product.Name,
                ProductPrice = skus[i.Sku].Price,
                ProductSellingPrice = skus[i.Sku].SellingPrice,
                ProductShortDescription = skus[i.Sku].ShortDescription,
                Quantity = i.Quantity,
                SKU = i.Sku
            }).ToArray()
        };
    }

    public Task<Exception?> Clear(Guid cartId)
    {
        DataMocking.Carts.FirstOrDefault(c => c.Id == cartId)?
            .Items.Clear();

        return Task.FromResult(null as Exception);
    }

    public async Task<(CartResponseVM?, Exception?)> Create()
    {
        await Task.CompletedTask;

        Models.Cart cart = new() { Id = Guid.NewGuid() };

        DataMocking.Carts.Add(cart);

        return (ToCartResponse(cart), null);
    }

    public async Task<(CartResponseVM?, Exception?)> Create(CreateCartWithItemsRequestVM request)
    {
        await Task.CompletedTask;

        var product = DataMocking.Products.SelectMany(p => p.Variations)
            .SingleOrDefault(p => p.Sku == request.Sku);

        if (product is null)
            return (null, new InvalidDataException("Sku not found"));

        Models.Cart cart = new()
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Items = new() { new() { Id = product.Id, Quantity = request.Quantity, Sku = request.Sku } }
        };

        DataMocking.Carts.Add(cart);

        return (ToCartResponse(cart), null);
    }

    public async Task<Exception?> DeleteItemBySKU(Guid cartId, string sku)
    {
        await Task.CompletedTask;

        var cart = DataMocking.Carts.FirstOrDefault(c => c.Id == cartId);

        if (cart is null)
            return null;

        var item = cart.Items.FirstOrDefault(x => x.Sku == sku);

        if (item is not null)
            cart.Items.Remove(item);

        return null;
    }

    public async Task<(CartResumeResponseVM?, Exception?)> Resume(Guid cartId)
    {
        await Task.CompletedTask;

        var cart = DataMocking.Carts.FirstOrDefault(c => c.Id == cartId);

        if (cart is null)
        {
            var data = await Create();

            cartId = data.Item1!.Value.Id;

            cart = DataMocking.Carts.FirstOrDefault(c => c.Id == cartId);
        }

        return (ToCartResumeResponse(cart!), null);
    }

    public async Task<(CartResponseVM?, Exception?)> OpenCart(Guid cartId)
    {
        await Task.CompletedTask;

        var cart = DataMocking.Carts.FirstOrDefault(c => c.Id == cartId);

        if (cart is null)
            return (null, null);

        return (ToCartResponse(cart), null);
    }

    public async Task<(CartResponseVM?, Exception?)> ReOpen(Guid userId)
    {
        await Task.CompletedTask;

        var cart = DataMocking.Carts.FirstOrDefault(c => c.UserId == userId);

        if (cart is null)
        {
            var creation = await Create();

            await SetUser(creation.Item1!.Value.Id, new CartSetUserRequestVM { UserId = userId });

            return creation;
        }

        return (ToCartResponse(cart), null);
    }

    public async Task<Exception?> SetUser(Guid cartId, CartSetUserRequestVM request)
    {
        await Task.CompletedTask;

        var cart = DataMocking.Carts.FirstOrDefault(c => c.Id == cartId);

        if (cart is null)
            return new InvalidDataException("Cart not found");

        cart.UserId = request.UserId;

        return null;
    }
}
