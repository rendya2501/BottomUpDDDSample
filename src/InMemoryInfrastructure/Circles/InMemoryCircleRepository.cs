using Domain.Domain.Circles;
using Domain.Domain.Users;
using ProductInfrastructure.Circles;

namespace InMemoryInfrastructure.Circles;

public class InMemoryCircleRepository : ICircleRepository
{
    private readonly Dictionary<CircleId, Circle> data = [];

    public Circle? Find(CircleId id)
    {
        if (data.TryGetValue(id, out var target))
        {
            return CloneCircle(target);
        }
        return null;
    }

    public void Save(Circle circle)
    {
        var model = GetCircleDataModel(circle);
        data[new CircleId(model.Id)] = circle;
    }

    private Circle CloneCircle(Circle circle)
    {
        var model = GetCircleDataModel(circle);
        return new Circle(new CircleId(model.Id), new CircleName(model.Name), new List<UserId>(model.UserIds.Select(s => new UserId(s))));
    }

    private CircleDataModel GetCircleDataModel(Circle circle)
    {
        // サークル通知オブジェクトを作成する
        var note = new CircleNotification();
        // サークルエンティティのメンバー情報をサークル通知オブジェクト(note)に格納させる
        circle.Notify(note);
        // 通知情報をビルドしてデータとして受け取る
        return note.Build();
    }
}
