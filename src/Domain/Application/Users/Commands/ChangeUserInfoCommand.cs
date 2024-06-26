namespace Domain.Application.Users.Commands;

/// <summary>
/// ユーザ情報変更コマンド
/// </summary>
/// <param name="id"></param>
/// <param name="username"></param>
/// <param name="firstname"></param>
/// <param name="familyname"></param>
public class ChangeUserInfoCommand(
    string id,
    string username,
    string firstname,
    string familyname
)
{
    public string Id { get; } = id;
    public string UserName { get; } = username;
    public string FirstName { get; } = firstname;
    public string FamilyName { get; } = familyname;
}
