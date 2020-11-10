using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;

namespace MovingCastles.Maps
{
    /// <summary>
    /// <para>
    /// Extended version of DefaultFOVVisibilityHandler from GoRogueHelpers. Allows certain layers to be marked as
    /// explorable.
    /// </para>
    /// <para>
    /// Original summary:
    /// Handler that will make all terrain/entities inside FOV visible as normal, all entities outside of FOV invisible, all
    /// terrain outside of FOV invisible if unexplored, and set its foreground to <see cref="ExploredColor"/> if explored but out of FOV.
    /// </para>
    /// </summary>
    public class McFovVisibilityHandler : FOVVisibilityHandler
    {
        private const string GhostName = "FOV ghost";

        private readonly uint _explorableLayerMask;
        private readonly int _ghostLayer;

        /// <summary>
        /// Creates a DefaultFOVVisibilityHandler that will manage visibility of objects for the given map as noted in the class description.
        /// </summary>
        /// <param name="map">The map this handler will manage visibility for.</param>
        /// <param name="explorableLayerMask">Foreground color to set to all terrain tiles that are outside of FOV but have been explored.</param>
        /// <param name="startingState">The starting state to put the handler in.</param>
        public McFovVisibilityHandler(BasicMap map, Color unexploredColor, uint explorableLayerMask, int ghostLayer, State startingState = State.Enabled)
            : base(map, startingState)
        {
            _explorableLayerMask = explorableLayerMask;
            ExploredColor = unexploredColor;
            _ghostLayer = ghostLayer;
        }

        /// <summary>
        /// Foreground color to set to all terrain that is outside of FOV but has been explored.
        /// </summary>
        public Color ExploredColor { get; }

        /// <summary>
        /// Makes entity visible.
        /// </summary>
        /// <param name="entity">Entity to modify.</param>
        protected override void UpdateEntitySeen(BasicEntity entity)
        {
            if (entity.Layer == _ghostLayer)
            {
                Map.RemoveEntity(entity);
                return;
            }

            entity.IsVisible = true;
        }

        /// <summary>
        /// Makes entity invisible.
        /// </summary>
        /// <param name="entity">Entity to modify.</param>
        protected override void UpdateEntityUnseen(BasicEntity entity)
        {
            if (entity.Layer == _ghostLayer)
            {
                return;
            }

            if (LayerMasker.DEFAULT.HasLayer(_explorableLayerMask, entity.Layer)
                && Map.Explored[entity.Position])
            {
                // spawn ghost
                var ghost = new BasicEntity(
                    ExploredColor,
                    Color.Transparent,
                    entity.Animation[0].Glyph,
                    entity.Position,
                    _ghostLayer,
                    true,
                    true);
                ghost.IsVisible = true;
                ghost.Font = entity.Font;
                ghost.OnCalculateRenderPosition();
                ghost.Name = GhostName + " - " + entity.Name;

                Map.AddEntity(ghost);
            }

            entity.IsVisible = false;
        }

        /// <summary>
        /// Makes terrain visible and sets its foreground color to its regular value.
        /// </summary>
        /// <param name="terrain">Terrain to modify.</param>
        protected override void UpdateTerrainSeen(BasicTerrain terrain)
        {
            terrain.IsVisible = true;
            terrain.RestoreState();
        }

        /// <summary>
        /// Makes terrain invisible if it is not explored.  Makes terrain visible but sets its foreground to
        /// <see cref="ExploredColor"/> if it is explored.
        /// </summary>
        /// <param name="terrain">Terrain to modify.</param>
        protected override void UpdateTerrainUnseen(BasicTerrain terrain)
        {
            if (Map.Explored[terrain.Position])
            {
                terrain.SaveState();
                terrain.Foreground = ExploredColor;
            }
            else
            {
                terrain.IsVisible = false;
            }
        }
    }
}
