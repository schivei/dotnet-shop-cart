namespace System;

/// <summary>
/// BDD UserId
/// </summary>
public readonly struct UserId : IComparable, IComparable<UserId>, IEquatable<UserId>, ISpanFormattable, IFormattable
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public UserId(Guid guid) =>
        _value = guid;

    private readonly Guid _value;

    public int CompareTo(object? obj) =>
        _value.CompareTo(obj);

    public int CompareTo(UserId other) =>
        _value.CompareTo(other._value);

    public bool Equals(UserId other) =>
        _value.Equals(other._value);

    public string ToString(string? format, IFormatProvider? formatProvider) =>
        _value.ToString(format, formatProvider);

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? _) =>
        _value.TryFormat(destination, out charsWritten, format);

    public override bool Equals(object? obj) =>
        _value.Equals(obj);

    public override int GetHashCode() =>
        _value.GetHashCode();

    public static bool operator ==(UserId left, UserId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(UserId left, UserId right)
    {
        return !(left == right);
    }

    public static bool operator <(UserId left, UserId right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(UserId left, UserId right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(UserId left, UserId right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(UserId left, UserId right)
    {
        return left.CompareTo(right) >= 0;
    }

    public static implicit operator Guid(UserId cartId) =>
        cartId._value;

    public static implicit operator UserId(Guid guid) =>
        new(guid);

    public static UserId NewUserId() =>
        new(Guid.NewGuid());
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
