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
        // �T�[�N���ʒm�I�u�W�F�N�g���쐬����
        var note = new CircleNotification();
        // �T�[�N���G���e�B�e�B�̃����o�[�����T�[�N���ʒm�I�u�W�F�N�g(note)�Ɋi�[������
        circle.Notify(note);
        // �ʒm�����r���h���ăf�[�^�Ƃ��Ď󂯎��
        return note.Build();
    }
}
