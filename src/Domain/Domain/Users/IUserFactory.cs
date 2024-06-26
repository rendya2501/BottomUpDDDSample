namespace Domain.Domain.Users;

/// <summary>
/// ユーザーファクトリーインターフェース
/// </summary>
public interface IUserFactory {
    User CreateUser(UserName username, FullName fullName);
}
