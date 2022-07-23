namespace Schivei.Shop.Cart.Infrastructure.DTOs.Responses;

/// <summary>
/// Common Checkout response data
/// </summary>
public struct CheckoutResponseVM
{
    /// <summary>
    /// Subtotal
    /// </summary>
    public decimal Subtotal { get; set; }

    /// <summary>
    /// Discount
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Delivery Price
    /// </summary>
    public decimal Delivery { get; set; }

    /// <summary>
    /// Total
    /// </summary>
    public decimal Total { get; set; }
}
