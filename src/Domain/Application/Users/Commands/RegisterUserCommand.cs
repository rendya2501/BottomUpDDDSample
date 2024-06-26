namespace Domain.Application.Users.Commands;

/// <summary>
/// ユーザー登録コマンド
/// </summary>
/// <param name="username"></param>
/// <param name="firstname"></param>
/// <param name="familyname"></param>
public class RegisterUserCommand(
    string username,
    string firstname,
    string familyname
    )
{
    public string UserName { get; } = username;
    public string FirstName { get; } = firstname;
    public string FamilyName { get; } = familyname;
}
