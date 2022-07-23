using System.ComponentModel.DataAnnotations;

namespace Schivei.Shop.Cart.Infrastructure.DTOs.Requests;

/// <summary>
/// End checkout and clear cart
/// </summary>
public struct CheckoutDoPaymentRequestVM
{
    /// <summary>
    /// 16 digits Card Number
    /// </summary>
    [Required]
    [MinLength(16)]
    [MaxLength(16)]
    [RegularExpression(@"([0-9]+)", ErrorMessage = "Only numbers are allowed on card number.")]
    public string CardNumber { get; set; }

    /// <summary>
    /// 3-4 digits security card code
    /// </summary>
    [Required]
    [MinLength(3)]
    [MaxLength(4)]
    public string CvcCode { get; set; }

    /// <summary>
    /// Name equals printed at card
    /// </summary>
    [Required]
    public string CardName { get; set; }
}
