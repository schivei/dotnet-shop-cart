namespace System;

/// <summary>
/// BDD CartId
/// </summary>
public readonly struct CartId : IComparable, IComparable<CartId>, IEquatable<CartId>, ISpanFormattable, IFormattable
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public CartId(Guid guid) =>
        _value = guid;

    private readonly Guid _value;

    public int CompareTo(object? obj) =>
        _value.CompareTo(obj);

    public int CompareTo(CartId other) =>
        _value.CompareTo(other._value);

    public bool Equals(CartId other) =>
        _value.Equals(other._value);

    public string ToString(string? format, IFormatProvider? formatProvider) =>
        _value.ToString(format, formatProvider);

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? _) =>
        _value.TryFormat(destination, out charsWritten, format);

    public override bool Equals(object? obj) =>
        _value.Equals(obj);

    public override int GetHashCode() =>
        _value.GetHashCode();

    public static bool operator ==(CartId left, CartId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(CartId left, CartId right)
    {
        return !(left == right);
    }

    public static bool operator <(CartId left, CartId right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(CartId left, CartId right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(CartId left, CartId right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(CartId left, CartId right)
    {
        return left.CompareTo(right) >= 0;
    }

    public static implicit operator Guid(CartId cartId) =>
        cartId._value;

    public static implicit operator CartId(Guid guid) =>
        new(guid);

    public static CartId NewCartId() =>
        new(Guid.NewGuid());
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
