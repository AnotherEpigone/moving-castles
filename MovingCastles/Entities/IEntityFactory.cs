using GoRogue;
using MovingCastles.GameSystems.Items;
using MovingCastles.GameSystems.Player;

namespace MovingCastles.Entities
{
    public interface IEntityFactory
    {
        McEntity CreateActor(Coord position, ActorTemplate actorTemplate);
        McEntity CreateItem(Coord position, ItemTemplate itemTemplate);
        Wizard CreatePlayer(Coord position, Player playerInfo);
        Castle CreateCastle(Coord position, Player playerInfo);
        McEntity CreateDoor(Coord position);
    }
}