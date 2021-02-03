﻿using GoRogue;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Combat;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;

namespace MovingCastles.GameSystems.Spells.SpellEffects
{
    public class TeleportToTargetSpellEffect : ISpellEffect
    {
        public void Apply(
            IDungeonMaster dungeonMaster,
            McEntity caster,
            SpellTemplate spell,
            DungeonMap map,
            HitResult hitResult,
            Coord targetCoord,
            ILogManager logManager)
        {
            logManager.StoryLog($"{caster.ColoredName} teleported using {spell.Name}.");
            caster.Position = targetCoord;
        }
    }
}
