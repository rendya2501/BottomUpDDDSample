using System;
using Domain.Domain.Circles;

namespace Domain.Domain.Users;

/// <summary>
/// ユーザー_エンティティ
/// </summary>
public class User(UserId id, UserName userName, FullName name) : IEquatable<User>
{
    private readonly UserId id = id;

    public UserId Id => id;

    public UserName UserName => userName;

    public FullName Name => name;

    /// <summary>
    /// ユーザー名変更
    /// </summary>
    /// <param name="newName"></param>
    public void ChangeUserName(UserName newName)
    {
        ArgumentNullException.ThrowIfNull(newName);
        userName = newName;
    }

    /// <summary>
    /// 名前変更
    /// </summary>
    /// <param name="newName"></param>
    public void ChangeName(FullName newName)
    {
        ArgumentNullException.ThrowIfNull(newName);
        name = newName;
    }

    /// <summary>
    /// サークル作成
    /// </summary>
    /// <param name="circleFactory"></param>
    /// <param name="circleName"></param>
    /// <returns></returns>
    /// <remarks>
    /// 「サークルはユーザが作る」という事を表現する
    /// </remarks>
    public Circle CreateCircle(ICircleFactory circleFactory, CircleName circleName)
    {
        return circleFactory.Create(id, circleName);
    }

    public bool Equals(User other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Equals(id, other.id);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((User)obj);
    }

    public override int GetHashCode()
    {
        return (id != null ? id.GetHashCode() : 0);
    }
}