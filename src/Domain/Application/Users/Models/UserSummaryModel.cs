using Domain.Domain.Users;

namespace Domain.Application.Users.Models;

/// <summary>
/// 一覧用モデル
/// </summary>
public class UserSummaryModel(User source)
{
    public string Id { get; } = source.Id.Value;
    public string UserName { get; } = source.UserName.Value;
}
