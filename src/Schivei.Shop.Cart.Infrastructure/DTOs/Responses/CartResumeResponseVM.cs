namespace Schivei.Shop.Cart.Infrastructure.DTOs.Responses;

/// <summary>
/// Cart resume data
/// </summary>
public struct CartResumeResponseVM
{
    /// <summary>
    /// Items Count
    /// </summary>
    public int Items { get; set; }
    /// <summary>
    /// Buy subtotal
    /// </summary>
    public decimal Subtotal { get; set; }
    /// <summary>
    /// Cart ID
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// User ID
    /// </summary>
    public Guid? UserId { get; set; }
}
