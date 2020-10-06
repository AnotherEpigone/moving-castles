﻿using MovingCastles.GameSystems.Items;
using System.Collections.Generic;

namespace MovingCastles.GameSystems.PlayerInfo
{
    public class PlayerInfo
    {
        public string Name { get; set; }

        public float Health { get; set; }

        public float MaxHealth { get; set; }

        public List<ItemTemplate> Items { get; set; }

        public static PlayerInfo CreateDefault() => new PlayerInfo()
        {
            Health = 100,
            MaxHealth = 100,
            Items = new List<ItemTemplate>(),
        };
    }
}
