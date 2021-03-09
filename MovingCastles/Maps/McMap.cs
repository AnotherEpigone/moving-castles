using System;
using System.Linq;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using MovingCastles.Components.Stats;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Spells;
using SadConsole;

namespace MovingCastles.Maps
{
    public enum DungeonMapLayer
    {
        TERRAIN,
        DOODADS,
        GHOSTS,
        ITEMS,
        ACTORS,
        PLAYER
    }

    public class McMap : BasicMap
    {
        private readonly Lazy<Wizard> _player;

        public McMap(int width, int height)
            : base(
                  width,
                  height,
                  Enum.GetNames(typeof(DungeonMapLayer)).Length - 1,
                  Distance.CHEBYSHEV,
                  entityLayersSupportingMultipleItems: LayerMasker.DEFAULT.Mask((int)DungeonMapLayer.ITEMS, (int)DungeonMapLayer.GHOSTS))
        {
            // Note that passing *this* into the FOV handler sets up all kinds of FOV stuff in gorogue.
            // don't remove even if the property isn't used.
            FovVisibilityHandler = new McFovVisibilityHandler(
                this,
                ColorAnsi.BlackBright,
                LayerMasker.DEFAULT.Mask((int)DungeonMapLayer.ITEMS, (int)DungeonMapLayer.DOODADS),
                (int)DungeonMapLayer.GHOSTS);
            _player = new Lazy<Wizard>(() => Entities.Items.OfType<Wizard>().First());
        }

        public McFovVisibilityHandler FovVisibilityHandler { get; }

        public Wizard Player => _player.Value;

        public McEntity GetMonster(Coord target)
        {
            return GetEntity<McEntity>(target, LayerMasker.DEFAULT.Mask((int)DungeonMapLayer.ACTORS));
        }

        public McEntity GetActor(Coord target)
        {
            return GetEntity<McEntity>(target, LayerMasker.DEFAULT.Mask((int)DungeonMapLayer.ACTORS, (int)DungeonMapLayer.PLAYER));
        }

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

        public void ApplyTerrainOverlay<T>(IMapView<T> overlay, Coord position, Func<Coord, T, IGameObject> translator)
        {
            foreach (var overlayPos in overlay.Positions())
            {
                var terrainVal = overlay[overlayPos];
                var adjustedPos = overlayPos + position;
                SetTerrain(translator(adjustedPos, terrainVal));
            }
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
                var entity = GetMonster(target);
                return entity?.HasGoRogueComponent<IHealthComponent>() ?? false;
            }

            return WalkabilityView[target];
        }
    }
}
