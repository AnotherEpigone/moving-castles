using GoRogue;
using MovingCastles.GameSystems.Items;

namespace MovingCastles.Entities
{
    public interface IEntityFactory
    {
        McEntity CreateActor(Coord position, ActorTemplate actorTemplate);
        McEntity CreateDoodad(Coord position, DoodadTemplate template);
        McEntity CreateDoor(Coord position);
        McEntity CreateItem(Coord position, ItemTemplate itemTemplate);
        McEntity CreateItem(Coord position, Item item);
        Wizard CreatePlayer(Coord position, GameSystems.Player.WizardTemplate playerInfo);
    }
}