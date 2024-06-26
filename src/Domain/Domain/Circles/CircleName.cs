using System;

namespace Domain.Domain.Circles;

/// <summary>
/// サークル名_値オブジェクト
/// </summary>
public class CircleName : IEquatable<CircleName>
{
    public string Value { get; }

    public CircleName(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value.Length < 3) throw new ArgumentException("サークル名は3文字以上です。", nameof(value));
        if (value.Length > 20) throw new ArgumentException("サークル名は20文字以下です。", nameof(value));

        Value = value;
    }

    public bool Equals(CircleName other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(Value, other.Value);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((CircleName)obj);
    }

    public override int GetHashCode()
    {
        return (Value != null ? Value.GetHashCode() : 0);
    }
}
