using System.Diagnostics.CodeAnalysis;

namespace Schivei.Shop.Cart.Models;

/// <summary>
/// The cart item definition
/// </summary>
public class CartItem : IEqualityComparer<CartItem>, IEquatable<CartItem>
{
    /// <summary>
    /// The Cart Item ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The Product Id
    /// </summary>
    public string Sku { get; set; } = string.Empty;

    /// <summary>
    /// Number of items
    /// </summary>
    public uint Quantity { get; set; }

    public bool Equals(CartItem? other) =>
        other is not null && other.Id == Id;

    public bool Equals(CartItem? x, CartItem? y) =>
        x is not null && x.Equals(y);

    public override int GetHashCode() =>
        HashCode.Combine(Id);

    public int GetHashCode([DisallowNull] CartItem obj) =>
        obj is null ? 0 : obj.GetHashCode();

    public override bool Equals(object? obj) =>
        obj is CartItem c && Equals(c);
}
