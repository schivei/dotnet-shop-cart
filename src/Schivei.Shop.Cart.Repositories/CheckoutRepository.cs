using Schivei.Shop.Cart.Infrastructure.DTOs.Requests;
using Schivei.Shop.Cart.Infrastructure.DTOs.Responses;
using Schivei.Shop.Cart.Infrastructure.Repositories;
using Schivei.Shop.Cart.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Schivei.Shop.Cart.Repositories;

internal class CheckoutRepository : ICheckoutRepository
{
    public async Task<(CheckoutResponseVM?, Exception?)> Calculate(UserId userId, CheckoutRequestVM request)
    {
        await Task.CompletedTask;

        var cart = Get(userId);

        cart ??= Create(Guid.NewGuid(), userId);

        var subtotal = cart.Items.Sum(i => GetSku(i.Sku)!.SellingPrice * i.Quantity);

        var zip = request.ZipCode.Trim('0');

        CheckoutResponseVM response = new()
        {
            Delivery = decimal.Parse(zip[0] + "." + zip[1], CultureInfo.InvariantCulture),
            Discount = GetDiscount(request.Coupon) * subtotal,
            Subtotal = subtotal
        };

        response.Total = response.Subtotal - response.Discount + response.Delivery;

        return (response, null);
    }

    public async Task<(CartResponseVM?, Exception?)> DoPayment(UserId userId, CheckoutDoPaymentRequestVM _)
    {
        await Task.CompletedTask;

        var cart = Get(userId) ?? Create(Guid.NewGuid(), userId);

        if (cart.Items.Count == 0)
            return (null, new InvalidDataException("Cart is empty"));

        var items = cart.Items.ToArray();

        var notAvailableSkus = new ProductVariation[items.Length];
        int j = -1;
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            var sku = GetSku(item.Sku)!;
            if (sku.Stock - item.Quantity >= 0)
                continue;

            notAvailableSkus[++j] = sku;

            sku.Stock -= item.Quantity;
        }
        j++;

        if (j > 0)
            return (null, GetError(notAvailableSkus[..j]));

        cart.Items.Clear();

        return (CartRepository.ToCartResponse(cart), null);
    }

    private static Exception GetError(params ProductVariation[] notAvailableSkus)
    {
        var errors = new Exception[notAvailableSkus.Length];
        for (var i = 0; i < notAvailableSkus.Length; i++)
            errors[i] = new($"{notAvailableSkus[i].ShortDescription} is not available");

        return new AggregateException(errors);
    }

    private static Models.Cart Create(CartId cartId, UserId userId)
    {
        var cart = new Models.Cart { Id = cartId, UserId = userId };

        DataMocking.Carts.Add(cart);

        return cart;
    }

    private static ProductVariation? GetSku(string sku) =>
        DataMocking.SKUs.ContainsKey(sku) ? DataMocking.SKUs[sku] : null;

    private static Models.Cart? Get(UserId userId) =>
        DataMocking.Carts[userId];

    private static decimal GetDiscount(string? coupon) =>
        coupon is not null && Regex.IsMatch(coupon, "^(OFF)([0-9][0-9])$") ?
        (decimal.Parse(Regex.Replace(coupon, "^(OFF)([0-9][0-9])$", "$2")) / 100) : 0;
}