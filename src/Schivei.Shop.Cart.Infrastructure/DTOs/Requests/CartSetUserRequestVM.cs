using System.ComponentModel.DataAnnotations;

namespace Schivei.Shop.Cart.Infrastructure.DTOs.Requests;

/// <summary>
/// Set user to a cart
/// </summary>
public struct CartSetUserRequestVM
{
    /// <summary>
    /// User ID
    /// </summary>
    [Required]
    public Guid UserId { get; set; }
}
