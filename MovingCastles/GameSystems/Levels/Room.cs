using GoRogue;

namespace MovingCastles.GameSystems.Levels
{
    public enum RoomType
    {
        Rubble,
        Study,
    }

    public record Room(
        Rectangle Location,
        RoomType Type);
}
