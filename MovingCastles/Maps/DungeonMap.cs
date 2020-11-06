using System;
using System.Linq;
using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Spells;
using SadConsole;

namespace MovingCastles.Maps
{
    internal enum DungeonMapLayer
    {
        TERRAIN,
        DOODADS,
        ITEMS,
        MONSTERS,
        PLAYER
    }

    public class DungeonMap : McMap
    {
        private readonly Lazy<Wizard> _player;

        public DungeonMap(int width, int height)
            : base(
                  width,
                  height,
                  Enum.GetNames(typeof(DungeonMapLayer)).Length - 1,
                  Distance.CHEBYSHEV,
                  entityLayersSupportingMultipleItems: LayerMasker.DEFAULT.Mask((int)DungeonMapLayer.ITEMS))
        {
            // Note that passing *this* into the FOV handler sets up all kinds of FOV stuff in gorogue.
            // don't remove even if the property isn't used.
            FovVisibilityHandler = new DefaultFOVVisibilityHandler(this, ColorAnsi.BlackBright);
            _player = new Lazy<Wizard>(() => Entities.Items.OfType<Wizard>().First());
        }

        public FOVVisibilityHandler FovVisibilityHandler { get; }

        public Wizard Player => _player.Value;

        public (bool, Coord) GetTarget(Coord playerPos, Coord selectedTargetPos, ITargettingStyle targettingStyle)
        {
            var distance = Distance.CHEBYSHEV.Calculate(playerPos, selectedTargetPos);
            if (distance > targettingStyle.Range)
            {
                return (false, selectedTargetPos);
            }

            Coord target = selectedTargetPos;
            if (targettingStyle.TargetMode == TargetMode.Projectile)
            {
                target = GetProjectileTarget(playerPos, selectedTargetPos);
            }

            return (CheckTarget(target, targettingStyle), target);
        }

        private Coord GetProjectileTarget(Coord playerPos, Coord selectedTargetPos)
        {
            var line = Lines.Get(playerPos, selectedTargetPos, Lines.Algorithm.DDA);
            Coord target = playerPos;
            foreach (var point in line.Skip(1))
            {
                target = point;
                if (!WalkabilityView[point])
                {
                    break;
                }
            }

            return target;
        }

        private bool CheckTarget(Coord target, ITargettingStyle targettingStyle)
        {
            if (targettingStyle.Offensive)
            {
                var entity = GetEntity<McEntity>(target, LayerMasker.DEFAULT.Mask((int)DungeonMapLayer.MONSTERS));
                return entity?.HasGoRogueComponent<IHealthComponent>() ?? false;
            }

            return WalkabilityView[target];
        }
    }
}
