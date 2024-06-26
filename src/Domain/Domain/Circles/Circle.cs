using Domain.Domain.Users;
using System;
using System.Collections.Generic;

namespace Domain.Domain.Circles;

/// <summary>
/// サークル_エンティティ
/// </summary>
public class Circle(
    CircleId id,
    CircleName name,
    List<UserId> users
    ) : IEquatable<Circle>
{
    private readonly CircleId id = id;

    public bool Equals(Circle other)
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
        return Equals((Circle)obj);
    }

    public override int GetHashCode()
    {
        return (id != null ? id.GetHashCode() : 0);
    }

    /// <summary>
    /// サークルにユーザーを追加する
    /// </summary>
    /// <param name="user"></param>
    /// <exception cref="Exception"></exception>
    /// <remarks>
    /// ユーザの最大数は30人であるという知識がドメインモデルに記述することができている。
    /// </remarks>
    public void Join(User user)
    {
        if (users.Count >= 30)
        {
            throw new Exception("too many members.");
        }
        users.Add(user.Id);
    }

    /// <summary>
    /// サークル通知
    /// </summary>
    /// <param name="note"></param>
    public void Notify(ICircleNotification note)
    {
        note.Id(id);
        note.Name(name);
        note.Users(new List<UserId>(users));
    }
}
