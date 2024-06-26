using System;

namespace Domain.Domain.Users;

/// <summary>
/// ユーザー名_値オブジェクト
/// </summary>
public class UserName : IEquatable<UserName>
{
    private readonly string name;

    public string Value => name;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="name"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public UserName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException(nameof(name));
        }
        if (name.Length > 50)
        {
            throw new ArgumentOutOfRangeException(nameof(name), "It must be 50 characters or less");
        }
        this.name = name;
    }

    public bool Equals(UserName other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(name, other.name);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((UserName)obj);
    }

    public override int GetHashCode()
    {
        return (name != null ? name.GetHashCode() : 0);
    }
}
