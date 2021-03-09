using GoRogue;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Spells;
using MovingCastles.Maps;
using SadConsole.Input;
using System.Collections.Generic;

namespace MovingCastles.GameSystems.TurnBased
{
    public interface ITurnBasedGame
    {
        McMap Map { get; }
        State State { get; set; }
        SpellTemplate TargettingSpell { get; }
        List<Coord> TargetInteractables { get; }
        List<Coord> TargetableTiles { get; }

        bool HandleAsPlayerInput(Keyboard info);
        void RegisterEntity(McEntity entity);
        void UnregisterEntity(McEntity entity);
        void RegisterPlayer(Wizard player);
        void SpellTargetSelected(Coord mapCoord);
        void StartSpellTargetting(SpellTemplate spell);
        void InteractTargetSelected(Coord mapCoord);
        bool HandleAsInteractTargettingInput(Keyboard info);
        void Init(McMap map);
    }
}