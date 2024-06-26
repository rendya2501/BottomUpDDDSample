using Domain.Domain.Users;

namespace Domain.Application.Users.Models;

/// <summary>
/// ユーザーモデル
/// </summary>
public class UserModel(User source)
{
    public string Id { get; } = source.Id.Value;
    public string UserName { get; } = source.UserName.Value;
    public FullNameModel Name { get; } = new FullNameModel(source.Name);
}
