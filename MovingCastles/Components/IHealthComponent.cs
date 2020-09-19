﻿using GoRogue.GameFramework.Components;

namespace MovingCastles.Components
{
    public interface IHealthComponent : IGameObjectComponent
    {
        float Health { get; }
        float MaxHealth { get; }

        bool Dead { get; }

        void ApplyDamage(float damage);
        void ApplyHealing(float healing);
    }
}