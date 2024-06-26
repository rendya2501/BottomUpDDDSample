using System.Collections.Generic;
using System.Data;
using Domain.Domain.Users;
using MySql.Data.MySqlClient;

namespace ProductInfrastructure.Users;

/// <summary>
/// ユーザーリポジトリ
/// </summary>
public class UserRepository(IDbConnection connection) : IUserRepository
{
    public User Find(UserId id)
    {
        connection.Open();

        using var com = connection.CreateCommand();
        com.CommandText = "SELECT * FROM t_user WHERE id = @id";
        com.Parameters.Add(new MySqlParameter("@id", id.Value));
        var reader = com.ExecuteReader();

        if (reader.Read())
        {
            var username = reader["username"] as string;
            var firstname = reader["firstname"] as string;
            var familyname = reader["familyname"] as string;
            return new User(
                id,
                new UserName(username),
                new FullName(firstname, familyname)
            );
        }

        return null;
    }

    public User Find(UserName userName)
    {
        connection.Open();

        using var com = connection.CreateCommand();
        com.CommandText = "SELECT * FROM t_user WHERE username = @username";
        com.Parameters.Add(new MySqlParameter("@username", userName.Value));
        var reader = com.ExecuteReader();

        if (reader.Read())
        {
            var id = reader["id"] as string;
            var firstname = reader["firstname"] as string;
            var familyname = reader["familyname"] as string;

            return new User(
                new UserId(id),
                userName,
                new FullName(firstname, familyname)
            );
        }

        return null;
    }

    public IEnumerable<User> FindAll()
    {
        connection.Open();

        using var com = connection.CreateCommand();
        com.CommandText = "SELECT * FROM t_user";
        var reader = com.ExecuteReader();
        var results = new List<User>();

        while (reader.Read())
        {
            var id = reader["id"] as string;
            var firstname = reader["firstname"] as string;
            var familyname = reader["familyname"] as string;
            var username = reader["username"] as string;
            var user = new User(
                new UserId(id),
                new UserName(username),
                new FullName(firstname, familyname)
            );
            results.Add(user);
        }
        return results;
    }

    public void Save(User user)
    {
        connection.Open();

        bool isExist;
        using var com = connection.CreateCommand();
        com.CommandText = "SELECT * FROM t_user WHERE id = @id";
        com.Parameters.Add(new MySqlParameter("@id", user.Id.Value));
        var reader = com.ExecuteReader();
        isExist = reader.Read();


        using var command = connection.CreateCommand();
        command.CommandText = isExist
            ? "UPDATE t_user SET username = @username, firstname = @firstname, familyname = @familyname WHERE id = @id"
            : "INSERT INTO t_user VALUES(@id, @username, @firstname, @familyname)";
        command.Parameters.Add(new MySqlParameter("@id", user.Id.Value));
        command.Parameters.Add(new MySqlParameter("@username", user.UserName.Value));
        command.Parameters.Add(new MySqlParameter("@firstname", user.Name.FirstName));
        command.Parameters.Add(new MySqlParameter("@familyname", user.Name.FamilyName));
        command.ExecuteNonQuery();
    }

    public void Remove(User user)
    {
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM t_user WHERE id = @id";
        command.Parameters.Add(new MySqlParameter("@id", user.Id.Value));
        command.ExecuteNonQuery();
    }
}