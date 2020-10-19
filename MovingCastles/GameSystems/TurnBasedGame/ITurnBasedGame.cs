using GoRogue;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Spells;
using MovingCastles.Maps;

namespace MovingCastles.GameSystems.TurnBasedGame
{
    public interface ITurnBasedGame
    {
        DungeonMap Map { get; set; }
        State State { get; set; }
        SpellTemplate TargettingSpell { get; }

        bool HandleAsPlayerInput(SadConsole.Input.Keyboard info);
        void RegisterEntity(McEntity entity);
        void UnregisterEntity(McEntity entity);
        void RegisterPlayer(Wizard player);
        void TargetSelected(Coord mapCoord);
        void StartTargetting(SpellTemplate spell);
    }
}