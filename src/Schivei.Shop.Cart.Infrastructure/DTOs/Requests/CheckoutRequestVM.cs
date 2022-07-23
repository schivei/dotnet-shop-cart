using System.ComponentModel.DataAnnotations;

namespace Schivei.Shop.Cart.Infrastructure.DTOs.Requests;

/// <summary>
/// Get checkout info
/// </summary>
public struct CheckoutRequestVM
{
    /// <summary>
    /// Coupon code
    /// </summary>
    public string? Coupon { get; set; }

    /// <summary>
    /// Zip Code location
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    [MinLength(3)]
    [MaxLength(20)]
    public string ZipCode { get; set; }
}
