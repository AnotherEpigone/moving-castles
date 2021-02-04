﻿using GoRogue;
using MovingCastles.Components.Effects;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Combat;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
using System;

namespace MovingCastles.GameSystems.Spells.SpellEffects
{
    public class SpeedChangeSpellEffect : ISpellEffect
    {
        private readonly float _modifier;
        private readonly int _lifetimeSeconds;

        public SpeedChangeSpellEffect(float modifier, int lifetimeSeconds)
        {
            _modifier = modifier;
            _lifetimeSeconds = lifetimeSeconds;
        }

        public string Description => _modifier > 0
            ? $"Speeds target by {(int)(_modifier * 100)}% for {_lifetimeSeconds} seconds."
            : $"Slows target by {(int)(Math.Abs(_modifier) * 100)}% for {_lifetimeSeconds} seconds.";

        public void Apply(
            IDungeonMaster dungeonMaster,
            McEntity caster,
            SpellTemplate spell,
            DungeonMap map,
            HitResult hitResult,
            Coord targetCoord,
            ILogManager logManager)
        {
            if (hitResult == HitResult.Miss || hitResult == HitResult.Glance)
            {
                return;
            }

            var target = map.GetEntity<McEntity>(targetCoord);
            var targetName = target.ColoredName;

            logManager.CombatLog($"{targetName} was slowed.");
            target.AddGoRogueComponent(new SpeedChangeTimedEffect(dungeonMaster.TimeMaster.JourneyTime, _modifier, _lifetimeSeconds * 100));
        }
    }
}