using MovingCastles.GameSystems.Items;
using System.Collections.Generic;

namespace MovingCastles.GameSystems.Player
{
    public class Player
    {
        public string Name { get; set; }

        public float Health { get; set; }

        public float MaxHealth { get; set; }

        public List<ItemTemplate> Items { get; set; }

        public static Player CreateDefault() => new Player()
        {
            Health = 100,
            MaxHealth = 100,
            Items = new List<ItemTemplate>(),
        };
    }
}
