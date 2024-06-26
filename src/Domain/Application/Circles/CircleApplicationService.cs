using Domain.Domain.Circles;
using Domain.Domain.Users;
using System;
using System.Transactions;

namespace Domain.Application.Circles;

/// <summary>
/// サークル_アプリケーションサービス
/// </summary>
public class CircleApplicationService(
    ICircleFactory circleFactory,
    ICircleRepository circleRepository,
    IUserRepository userRepository
    )
{
    /// <summary>
    /// サークルの作成
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="circleName"></param>
    /// <exception cref="Exception"></exception>
    public void CreateCircle(string userId, string circleName)
    {
        using var transaction = new TransactionScope();
        var ownerId = new UserId(userId);
        var owner = userRepository.Find(ownerId) ?? throw new Exception("owner not found. userId: " + userId);

        // ユーザーがサークルを作成する
        var circle = owner.CreateCircle(circleFactory, new CircleName(circleName));
        circleRepository.Save(circle);
        transaction.Complete();
    }

    /// <summary>
    /// ユーザーの追加
    /// </summary>
    /// <param name="circleId"></param>
    /// <param name="userId"></param>
    /// <exception cref="Exception"></exception>
    public void JoinUser(string circleId, string userId)
    {
        using var transaction = new TransactionScope();
        var targetCircleId = new CircleId(circleId);
        var targetCircle = circleRepository.Find(targetCircleId) ?? throw new Exception("circle not found. circleId: " + circleId);
        var joinUserId = new UserId(userId);
        var joinUser = userRepository.Find(joinUserId) ?? throw new Exception("user not found. userId: " + userId);

        targetCircle.Join(joinUser); // targetCircle.Users.Add(joinUser); とは書けなくなりました
        circleRepository.Save(targetCircle);
        transaction.Complete();
    }
}
