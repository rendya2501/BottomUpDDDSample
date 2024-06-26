using Domain.Application.Users.Commands;
using Domain.Application.Users.Models;
using Domain.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.UnitOfWorkSample;

public class UserApplicationServiceByUow(
    IUserFactory userFactory,
    IUnitOfWork uow)
{
    private readonly UserService userService = new(uow.UserRepository);

    public void RegisterUser(RegisterUserCommand command)
    {
        try
        {
            var user = userFactory.CreateUser(
                new UserName(command.UserName),
                new FullName(command.FirstName, command.FamilyName)
            );
            if (userService.IsDuplicated(user))
            {
                throw new Exception("重複しています");
            }

            uow.UserRepository.Save(user);
            uow.Commit();
        }
        catch
        {
            uow.Rollback();
            throw;
        }
    }

    public void ChangeUserInfo(ChangeUserInfoCommand command)
    {
        try
        {
            var targetId = new UserId(command.Id);
            var target = uow.UserRepository.Find(targetId) ?? throw new Exception("not found. target id:" + command.Id);
            var newUserName = new UserName(command.UserName);
            target.ChangeUserName(newUserName);
            var newName = new FullName(command.FirstName, command.FamilyName);
            target.ChangeName(newName);
            uow.UserRepository.Save(target);
            uow.Commit();
        }
        catch
        {
            uow.Rollback();
            throw;
        }
    }

    public void RemoveUser(string id)
    {
        try
        {
            var targetId = new UserId(id);
            var target = uow.UserRepository.Find(targetId) ?? throw new Exception("not found. target id:" + id);
            uow.UserRepository.Remove(target);
            uow.Commit();
        }
        catch
        {
            uow.Rollback();
            throw;
        }
    }

    public UserModel GetUserInfo(string id)
    {
        var userId = new UserId(id);
        var target = uow.UserRepository.Find(userId);
        if (target == null)
        {
            return null;
        }

        return new UserModel(target);
    }

    public List<UserSummaryModel> GetUserList()
    {
        var users = uow.UserRepository.FindAll();
        return users.Select(x => new UserSummaryModel(x)).ToList();
    }
}
