namespace Domain.Domain.Circles;

/// <summary>
/// サークルリポジトリインターフェース
/// </summary>
public interface ICircleRepository
{
    Circle Find(CircleId id);
    void Save(Circle circle);
}
