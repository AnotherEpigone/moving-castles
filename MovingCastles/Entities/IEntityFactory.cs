using GoRogue;
using MovingCastles.GameSystems.Items;

namespace MovingCastles.Entities
{
    public interface IEntityFactory
    {
        McEntity CreateActor(Coord position, ActorTemplate actorTemplate);
        McEntity CreateItem(Coord position, ItemTemplate itemTemplate);
        Wizard CreatePlayer(Coord position, GameSystems.Player.WizardTemplate playerInfo);
        McEntity CreateDoor(Coord position);
        McEntity CreateDoodad(Coord position, DoodadTemplate template);
    }
}