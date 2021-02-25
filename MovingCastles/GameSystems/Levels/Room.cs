using GoRogue;

namespace MovingCastles.GameSystems.Levels
{
    public enum RoomType
    {
        None,
        Rubble,
        Study,
        Stairwell,
        Storeroom,
        Lobby,
        Hallway,
    }

    public class Room
    {
        public Room(
            Rectangle Location,
            RoomType Type)
        {
            this.Location = Location;
            this.Type = Type;
        }

        public Rectangle Location { get; }
        public RoomType Type { get; set; }
    }
}
