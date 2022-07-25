namespace Schivei.Shop.Cart.Models;

/// <summary>
/// The cart definition
/// </summary>
public class Cart
{
    public Cart() =>
        Items = new HashSet<CartItem>();

    /// <summary>
    /// Object ID
    /// </summary>
    public CartId Id { get; set; }

    /// <summary>
    /// Options User ID
    /// </summary>
    public UserId? UserId { get; set; }

    /// <summary>
    /// List of cart item
    /// </summary>
    public HashSet<CartItem> Items { get; set; }
}
