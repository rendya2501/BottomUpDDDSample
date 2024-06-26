using Domain.Domain.Users;

namespace InMemoryInfrastructure.Users;

public class InMemoryUserRepository : IUserRepository
{
    private readonly Dictionary<UserId, User> data = [];

    public User? Find(UserId id)
    {
        if (data.TryGetValue(id, out var target))
        {
            return CloneUser(target);
        }

        return null;
    }

    public User? Find(UserName name)
    {
        var target = data.Values.FirstOrDefault(x => x.UserName.Equals(name));
        if (target != null)
        {
            return CloneUser(target);
        }

        return null;
    }

    public IEnumerable<User> FindAll()
    {
        return data.Values.Select(CloneUser);
    }

    public void Save(User user)
    {
        Task.Delay(1000).Wait();
        data[user.Id] = user;
    }

    public void Remove(User user)
    {
        data.Remove(user.Id);
    }

    private User CloneUser(User user)
    {
        return new User(user.Id, user.UserName, user.Name);
    }
}