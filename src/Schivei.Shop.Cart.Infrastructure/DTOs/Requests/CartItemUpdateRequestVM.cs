using System.ComponentModel.DataAnnotations;

namespace Schivei.Shop.Cart.Infrastructure.DTOs.Requests;

/// <summary>
/// Cart item update quantity
/// </summary>
public struct CartItemUpdateRequestVM
{
    /// <summary>
    /// Product SKU
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string Sku { get; set; }

    /// <summary>
    /// Quantity of products to buy
    /// </summary>
    [Range(0, uint.MaxValue)]
    public uint Quantity { get; set; }
}
