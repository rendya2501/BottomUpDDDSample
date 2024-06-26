using System;

namespace Domain.Domain.Circles;

/// <summary>
/// サークルID_値オブジェクト
/// </summary>
public class CircleId(string value) : IEquatable<CircleId>
{
    public string Value { get; } = value;

    public bool Equals(CircleId other)
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
        return Equals((CircleId)obj);
    }

    public override int GetHashCode()
    {
        return (Value != null ? Value.GetHashCode() : 0);
    }
}
