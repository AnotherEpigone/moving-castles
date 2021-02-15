using GoRogue;

namespace MovingCastles.GameSystems.Levels
{
    public enum RoomType
    {
        Rubble,
    }

    public record Room(
        Rectangle Location,
        RoomType Type);
}
