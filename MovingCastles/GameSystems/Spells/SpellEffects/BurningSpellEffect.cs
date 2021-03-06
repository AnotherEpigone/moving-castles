﻿using GoRogue;
using MovingCastles.Components.Effects;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Combat;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;

namespace MovingCastles.GameSystems.Spells.SpellEffects
{
    public class BurningSpellEffect : ISpellEffect
    {
        private readonly float _dps;
        private readonly int _lifetimeSeconds;

        public BurningSpellEffect(float dps, int lifetimeSeconds)
        {
            _dps = dps;
            _lifetimeSeconds = lifetimeSeconds;
        }

        public string Description => $"Sets target aflame on a hit or crit. Burns for {_dps:0.#} damage per second for {_lifetimeSeconds} seconds.";

        public void Apply(
            IDungeonMaster dungeonMaster,
            McEntity caster,
            SpellTemplate spell,
            McMap map,
            HitResult hitResult,
            Coord targetCoord,
            ILogManager logManager)
        {
            if (hitResult == HitResult.Miss || hitResult == HitResult.Glance)
            {
                return;
            }

            var target = map.GetActor(targetCoord);
            if (target == null)
            {
                return;
            }

            var targetName = target.ColoredName;

            var effectiveDps = hitResult == HitResult.Crit
                ? _dps * 2
                : _dps;

            logManager.CombatLog($"{targetName} was set aflame.");
            target.AddGoRogueComponent(new BurningTimedEffect(dungeonMaster.TimeMaster.JourneyTime, effectiveDps, _lifetimeSeconds * 100));
        }
    }
}
