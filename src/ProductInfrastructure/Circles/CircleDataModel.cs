using System.Collections.Generic;

namespace ProductInfrastructure.Circles;

/// <summary>
/// サークルデータモデル
/// </summary>
/// <param name="id"></param>
/// <param name="name"></param>
/// <param name="userIds"></param>
public class CircleDataModel(string id, string name, List<string> userIds)
{
    public string Id { get; } = id;
    public string Name { get; } = name;
    public List<string> UserIds { get; } = userIds;
}


// 備忘録
// プライマリーコンストラクタを使わない場合の記述
//public class CircleDataModel
//{
//    public string Id { get; }
//    public string Name { get; }
//    public List<string> UserIds { get; }

//    public CircleDataModel(string id, string name, List<string> userIds)
//    {
//        Id = id;
//        Name = name;
//        UserIds = userIds;
//    }
//}
