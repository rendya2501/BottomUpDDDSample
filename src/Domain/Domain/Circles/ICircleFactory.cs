using Domain.Domain.Users;

namespace Domain.Domain.Circles;

/// <summary>
/// インターフェース_サークルファクトリー
/// </summary>
public interface ICircleFactory
{
    Circle Create(UserId userId, CircleName circleName);
}
