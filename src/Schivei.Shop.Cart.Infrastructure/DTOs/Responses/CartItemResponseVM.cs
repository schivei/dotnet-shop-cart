namespace Schivei.Shop.Cart.Infrastructure.DTOs.Responses;

/// <summary>
/// Cart item response data
/// </summary>
public struct CartItemResponseVM
{
    /// <summary>
    /// Product SKU
    /// </summary>
    public string SKU { get; set; }
    /// <summary>
    /// Product Name
    /// </summary>
    public string ProductName { get; set; }
    /// <summary>
    /// Product Image
    /// </summary>
    public string ProductImage { get; set; }
    /// <summary>
    /// Product Short Description
    /// </summary>
    public string ProductShortDescription { get; set; }
    /// <summary>
    /// Current price
    /// </summary>
    public decimal ProductPrice { get; set; }
    /// <summary>
    /// Current Selling Price (with promo discount)
    /// </summary>
    public decimal ProductSellingPrice { get; set; }
    /// <summary>
    /// Buing quantity
    /// </summary>
    public uint Quantity { get; set; }
}
