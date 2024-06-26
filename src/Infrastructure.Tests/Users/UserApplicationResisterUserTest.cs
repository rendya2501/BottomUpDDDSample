using Domain.Application;
using Domain.Application.Users.Commands;
using Domain.Domain.Users;
using InMemoryInfrastructure.Users;
using Moq;

namespace Infrastructure.Tests.Users;

public class UserApplicationResisterUserTest
{
    [Fact]
    public void TestRegister()
    {
        var repository = new InMemoryUserRepository();
        var factory = new InMemoryUserFactory();
        var app = new UserApplicationService(factory, repository);
        app.RegisterUser(new RegisterUserCommand("ttaro", "taro", "tanaka"));

        var user = repository.Find(new UserName("ttaro"));
        Assert.NotNull(user);
        Assert.True(user.Id.Value == "1");
        Assert.Contains("ttaro", user.UserName.Value);
    }

    [Fact]
    public void TestDuplicateFail()
    {
        var repository = new InMemoryUserRepository();
        var factory = new InMemoryUserFactory();
        var userName = new UserName("ttaro");
        var fullName = new FullName("taro", "tanaka");

        //var moq = new Mock<IUserFactory>();
        //moq.Setup(x => x.CreateUser(userName, fullName));
        //var moqfactory = moq.Object;
        //var ss = moqfactory.CreateUser(userName, fullName);

        // リポジトリにユーザーを登録しておく
        repository.Save(factory.CreateUser(userName, fullName));

        // アプリケーションサービスのユーザの登録で重複ユーザーを確認する
        // 事前に作成したユーザーと同じユーザー名で登録するため重複エラーが発生する
        Assert.Throws<Exception>(
            () => new UserApplicationService(factory, repository)
                .RegisterUser(new RegisterUserCommand("ttaro", "taro", "tanaka"))
        );

        //bool isOk = false;
        //try
        //{
        //    app.RegisterUser(new RegisterUserCommand("ttaro", "taro", "tanaka"));
        //}
        //catch (Exception ex)
        //{
        //    isOk = ex.Message.StartsWith("重複");
        //}
        //Assert.True(isOk);
    }

    [Fact]
    public void TestUserIsAtomic()
    {
        var repository = new InMemoryUserRepository();
        var factory = new InMemoryUserFactory();
        var app = new UserApplicationService(factory, repository);

        Parallel.For(0, 2, _ => app.RegisterUser(new RegisterUserCommand("ttaro", "taro", "tanaka")));
        var users = repository.FindAll();
        Assert.False(users.Count() == 1);
    }
}
