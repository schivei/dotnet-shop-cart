namespace Schivei.Shop.Cart.Infrastructure.DTOs.Responses;

/// <summary>
/// Common cart response data
/// </summary>
public struct CartResponseVM
{
    /// <summary>
    /// Cart ID
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// User ID
    /// </summary>
    public Guid? UserId { get; set; }
    /// <summary>
    /// Items to buy
    /// </summary>
    public CartItemResponseVM[] Items { get; set; }
}
