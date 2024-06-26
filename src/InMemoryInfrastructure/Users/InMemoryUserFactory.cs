using Domain.Domain.Users;

namespace InMemoryInfrastructure.Users;

public class InMemoryUserFactory : IUserFactory
{
    private static int _CurrentId;

    public User CreateUser(UserName username, FullName fullName)
    {
        // ユーザ が 生成 さ れる たび に インクリメント する
        _CurrentId++;

        return new User(
            new UserId(_CurrentId.ToString()),
            username,
            fullName
        );
    }
}
