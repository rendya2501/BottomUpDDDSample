using System.Collections.Generic;
using Domain.Domain.Users;

namespace Domain.Domain.Circles;

/// <summary>
/// サークル通知インターフェース
/// </summary>
public interface ICircleNotification
{
    void Id(CircleId id);
    void Name(CircleName circleName);
    void Users(List<UserId> users);
}
