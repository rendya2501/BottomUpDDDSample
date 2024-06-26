using Domain.Domain.Circles;
using Domain.Domain.Users;
using System.Collections.Generic;
using System.Linq;

namespace ProductInfrastructure.Circles;

/// <summary>
/// �T�[�N���ʒm�I�u�W�F�N�g
/// </summary>
public class CircleNotification : ICircleNotification
{
    private CircleId id;
    private CircleName name;
    private List<UserId> userIds;

    public void Id(CircleId id)
    {
        this.id = id;
    }

    public void Name(CircleName name)
    {
        this.name = name;
    }

    public void Users(List<UserId> userIds)
    {
        this.userIds = userIds;
    }

    /// <summary>
    /// �ʒm�I�u�W�F�N�g�̍쐬
    /// </summary>
    /// <returns></returns>
    public CircleDataModel Build()
    {
        var ids = userIds.Select(x => x.Value).ToList();

        return new CircleDataModel(
            id.Value,
            name.Value,
            ids
        );
    }
}
