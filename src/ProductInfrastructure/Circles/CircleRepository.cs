using Domain.Domain.Circles;
using Domain.Domain.Users;
using MySql.Data.MySqlClient;
using System.Data;
using System.Linq;

namespace ProductInfrastructure.Circles;

/// <summary>
/// サークルリポジトリ
/// </summary>
/// <param name="con"></param>
public class CircleRepository(IDbConnection con) : ICircleRepository
{
    /// <summary>
    /// サークルの検索
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Circle Find(CircleId id)
    {
        using var com = con.CreateCommand();
        com.CommandText = "SELECT * FROM t_circle WHERE id = @id";
        com.Parameters.Add(new MySqlParameter("@id", id.Value));
        var reader = com.ExecuteReader();
        if (reader.Read())
        {
            var circleName = reader["circle_name"] as CircleName;
            var users = (reader["join_members"] as string)?.Split(",").Select(s => new UserId(s)).ToList();
            return new Circle(id, circleName, users);
        }

        return null;
    }

    /// <summary>
    /// サークル情報の保存
    /// </summary>
    /// <param name="circle"></param>
    public void Save(Circle circle)
    {
        // サークルエンティティのメンバーは全てprivateとなっており(カプセル化)、直接アクセスする事が出来ない
        // そのため、通知オブジェクトを通してメンバー情報を取得する

        // サークル通知オブジェクトを作成する
        var note = new CircleNotification();
        // サークルエンティティのメンバー情報をサークル通知オブジェクト(note)に格納させる
        circle.Notify(note);
        // 通知情報をビルドしてデータとして受け取る
        var data = note.Build();

        using var com = con.CreateCommand();
        com.CommandText = @"
            UPDATE t_circle 
            SET circle_name = @circle_name, 
                join_members = @join_members
            WHERE id = @id";
        com.Parameters.Add(new MySqlParameter("@circle_name", data.Name));
        com.Parameters.Add(new MySqlParameter("@id", data.Id));
        com.Parameters.Add(new MySqlParameter("@join_members", string.Join(",", data.UserIds))); // 所属メンバーは中間テーブルにしたほうがいいかも
        com.ExecuteNonQuery();
    }
}
