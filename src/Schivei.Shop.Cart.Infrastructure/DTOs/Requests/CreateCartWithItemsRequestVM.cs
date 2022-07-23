using System.ComponentModel.DataAnnotations;

namespace Schivei.Shop.Cart.Infrastructure.DTOs.Requests;

/// <summary>
/// Create a new cart with an item
/// </summary>
public struct CreateCartWithItemsRequestVM
{
    /// <summary>
    /// User ID
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Product SKU
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string Sku { get; set; }

    /// <summary>
    /// Quantity to buy
    /// </summary>
    [Range(0, uint.MaxValue)]
    public uint Quantity { get; set; }
}
