using Domain.Application.Users.Commands;
using Domain.UnitOfWorkSample;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApplication.Models;

namespace WebApplication.Controllers;

public class UserController(UserApplicationServiceByUow userService) : Controller
{
    public IActionResult Index()
    {
        var users = userService.GetUserList();
        var summaries = users.Select(x => new UserSummaryViewModel
        {
            Id = x.Id,
            UserName = x.UserName
        });

        return View(summaries);
    }

    public IActionResult NewUser()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(RegisterUserViewModel model)
    {
        var username = model.UserName;
        var firstname = model.FirstName;
        var familyname = model.FamilyName;
        var registerUserCommand = new RegisterUserCommand(username, firstname, familyname);
        userService.RegisterUser(registerUserCommand);
        return Redirect("Index");
    }

    public IActionResult Delete(string id)
    {
        var user = userService.GetUserInfo(id);
        var model = new UserDetailViewModel
        {
            Id = user.Id,
            UserName = user.UserName,
            FirstName = user.Name.FirstName,
            FamilyName = user.Name.FamilyName,
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult DeleteUser(string id)
    {
        userService.RemoveUser(id);
        return Redirect("Index");
    }

    public IActionResult Detail(string id)
    {
        var user = userService.GetUserInfo(id);
        var model = new UserDetailViewModel
        {
            Id = user.Id,
            UserName = user.UserName,
            FirstName = user.Name.FirstName,
            FamilyName = user.Name.FamilyName,
        };
        return View(model);
    }

    public IActionResult Edit(string id)
    {
        var target = userService.GetUserInfo(id);
        var model = new EditUserViewModel
        {
            Id = target.Id,
            UserName = target.UserName,
            FirstName = target.Name.FirstName,
            FamilyName = target.Name.FamilyName,
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Update(EditUserViewModel model)
    {
        userService.ChangeUserInfo(new ChangeUserInfoCommand(model.Id, model.UserName, model.FirstName, model.FamilyName));
        return Redirect("Detail/" + model.Id);
    }
}
