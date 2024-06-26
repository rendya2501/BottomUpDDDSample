using Domain.Application.Users.Commands;
using Domain.Application.Users.Models;
using Domain.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Domain.Application;

/// <summary>
/// ユーザーアプリケーションサービス
/// </summary>
public class UserApplicationService(
    IUserFactory userFactory,
    IUserRepository userRepository
    )
{
    private readonly UserService userService = new(userRepository);

    /// <summary>
    /// ユーザの登録
    /// </summary>
    /// <param name="command"></param>
    /// <exception cref="Exception"></exception>
    public void RegisterUser(RegisterUserCommand command)
    {
        using var transaction = new TransactionScope();
        var user = userFactory.CreateUser(
            new UserName(command.UserName),
            new FullName(command.FirstName, command.FamilyName)
        );
        if (userService.IsDuplicated(user))
        {
            throw new Exception("重複しています");
        }

        userRepository.Save(user);
        transaction.Complete();
    }

    /// <summary>
    /// ユーザ情報変更
    /// </summary>
    /// <param name="command"></param>
    /// <exception cref="Exception"></exception>
    public void ChangeUserInfo(ChangeUserInfoCommand command)
    {
        var targetId = new UserId(command.Id);
        var target = userRepository.Find(targetId) ?? throw new Exception("not found. target id:" + command.Id);
        var newUserName = new UserName(command.UserName);
        target.ChangeUserName(newUserName);
        if (userService.IsDuplicated(target))
        {
            throw new Exception("重複しています");
        }

        var newName = new FullName(command.FirstName, command.FamilyName);
        target.ChangeName(newName);
        userRepository.Save(target);
    }

    /// <summary>
    /// ユーザの削除
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="Exception"></exception>
    public void RemoveUser(string id)
    {
        var targetId = new UserId(id);
        var target = userRepository.Find(targetId) ?? throw new Exception("not found. target id:" + id);
        userRepository.Remove(target);
    }

    /// <summary>
    /// ユーザ情報取得
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public UserModel GetUserInfo(string id)
    {
        var userId = new UserId(id);
        var target = userRepository.Find(userId);
        if (target == null)
        {
            return null;
        }
        return new UserModel(target);
    }

    /// <summary>
    /// ユーザ一覧取得
    /// </summary>
    /// <returns></returns>
    public List<UserSummaryModel> GetUserList()
    {
        var users = userRepository.FindAll();
        return users.Select(x => new UserSummaryModel(x)).ToList();
    }
}