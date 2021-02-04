using Microsoft.Xna.Framework;
using MovingCastles.Entities;
using System;

namespace MovingCastles.GameSystems.Movement
{
    public class EntityBumpedEventArgs : EventArgs
    {
        public EntityBumpedEventArgs(McEntity bumpingEntity, Point bumpedPosition)
        {
            BumpingEntity = bumpingEntity;
            BumpedPosition = bumpedPosition;
        }

        public bool MadeMeleeAttack { get; set; }
        public McEntity BumpingEntity { get; init; }
        public Point BumpedPosition { get; init; }
    }
}
