using System;

namespace Domain.Domain.Users;

/// <summary>
/// ユーザーID_値オブジェクト
/// </summary>
/// <param name="id"></param>
public class UserId(string id) : IEquatable<UserId>
{
    public string Value { get; } = id;

    public bool Equals(UserId other)
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
        return Equals((UserId)obj);
    }

    public override int GetHashCode()
    {
        return (Value != null ? Value.GetHashCode() : 0);
    }
}
