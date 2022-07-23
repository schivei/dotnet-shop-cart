using Schivei.Shop.Cart.Infrastructure.DTOs.Requests;
using Schivei.Shop.Cart.Infrastructure.DTOs.Responses;
using Schivei.Shop.Cart.Infrastructure.Repositories;
using Schivei.Shop.Cart.Models;
using System.Text.RegularExpressions;

namespace Schivei.Shop.Cart.Repositories;

internal class CheckoutRepository : ICheckoutRepository
{
    public async Task<(CheckoutResponseVM?, Exception?)> Calculate(Guid userId, CheckoutRequestVM request)
    {
        await Task.CompletedTask;

        var cart = DataMocking.Carts.FirstOrDefault(c => c.UserId == userId);

        if (cart is null)
            return (null, new InvalidDataException("Cart not found"));

        var skus = DataMocking.Products.SelectMany(p => p.Variations)
            .ToDictionary(p => p.Sku, p => p);

        var subtotal = cart.Items.Sum(i => skus[i.Sku].SellingPrice);

        CheckoutResponseVM response = new()
        {
            Delivery = decimal.Parse(DateTimeOffset.Now.AddMilliseconds(request.ZipCode.Sum(c => c)).ToString("m.s")),
            Discount = GetDiscount(request.Coupon, subtotal),
            Subtotal = subtotal
        };

        response.Total = response.Subtotal - response.Discount + response.Delivery;

        return (response, null);
    }

    private static decimal GetDiscount(string? coupon, decimal subtotal) =>
        coupon is not null && Regex.IsMatch(coupon, "^(OFF)([0-9][0-9])$") ?
        subtotal * decimal.Parse(Regex.Replace(coupon, "^(OFF)([0-9][0-9])$", "$2")) / 100 : 0;

    public async Task<(CartResponseVM?, Exception?)> DoPayment(Guid userId, CheckoutDoPaymentRequestVM _)
    {
        await Task.CompletedTask;

        var cart = DataMocking.Carts.FirstOrDefault(c => c.UserId == userId);

        if (cart is null)
            return (null, new InvalidDataException("Cart not found"));

        if (cart.Items.Count == 0)
            return (null, new InvalidDataException("Cart is empty"));

        var skus = DataMocking.Products.SelectMany(p => p.Variations)
            .ToDictionary(p => p.Sku, p => p);

        var notAvailableSkus = new List<ProductVariation>();

        foreach (var item in cart.Items)
        {
            var sku = skus[item.Sku];
            if (sku.Stock - item.Quantity < 0)
                notAvailableSkus.Add(sku);

            if (notAvailableSkus.Count > 0)
                continue;

            sku.Stock -= item.Quantity;
        }

        if (notAvailableSkus.Count > 0)
            return (null, new AggregateException(notAvailableSkus.Select(s => new InvalidDataException($"{s.ShortDescription} is not available")).ToArray()));

        cart.Items.Clear();

        return (CartRepository.ToCartResponse(cart), null);
    }
}