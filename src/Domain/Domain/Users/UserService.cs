namespace Domain.Domain.Users;

/// <summary>
/// ユーザードメインサービス
/// </summary>
public class UserService(IUserRepository userRepository)
{
    /// <summary>
    /// ユーザー名の重複確認を行う
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public bool IsDuplicated(User user)
    {
        var name = user.UserName;
        var searched = userRepository.Find(name);

        return searched != null;
    }
}


// プライマリーコンストラクタを使わない場合
//public class UserService
//{
//    private readonly IUserRepository userRepository;

//    public UserService(IUserRepository userRepository)
//    {
//        this.userRepository = userRepository;
//    }

//    public bool IsDuplicated(User user)
//    {
//        var name = user.UserName;
//        var searched = userRepository.Find(name);

//        return searched != null;
//    }
//}
