﻿using GoRogue.GameFramework;
using MovingCastles.Components.Triggers;
using MovingCastles.Entities;

namespace MovingCastles.Components
{
    public class OpenDoorComponent : IBumpTriggeredComponent
    {
        public IGameObject Parent { get; set; }

        public void Bump(McEntity bumpingEntity)
        {
            if (!(Parent is Door door))
            {
                return;
            }

            door.Open();
        }
    }
}
