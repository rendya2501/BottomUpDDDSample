using Domain.Domain.Users;

namespace Domain.Application.Users.Models;

/// <summary>
/// 完全名モデル
/// </summary>
public class FullNameModel(FullName source)
{
    public string FirstName { get; } = source.FirstName;
    public string FamilyName { get; } = source.FamilyName;
}
