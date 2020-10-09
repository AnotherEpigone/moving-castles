using Microsoft.Xna.Framework;
using MovingCastles.Fonts;
using System.Collections.Generic;

namespace MovingCastles.Entities
{
    public static class ActorAtlas
    {
        static ActorAtlas()
        {
            ActorsById = new Dictionary<string, ActorTemplate>
            {
                { Goblin.Id, Goblin },
                { Warg.Id, Warg }
            };
        }

        public static ActorTemplate Goblin => new ActorTemplate()
        {
            Id = "ACTOR_GOBLIN",
            Name = "Goblin",
            Glyph = SpriteAtlas.Goblin,
            NameColor = Color.DarkGreen,
            MaxHealth = 10,
            Health = 10,
            WalkSpeed = 1,
        };

        public static ActorTemplate Warg => new ActorTemplate()
        {
            Id = "ACTOR_WARG",
            Name = "Warg",
            Glyph = SpriteAtlas.Warg,
            NameColor = Color.DarkSlateGray,
            MaxHealth = 10,
            Health = 10,
            WalkSpeed = 2,
        };

        public static Dictionary<string, ActorTemplate>  ActorsById { get; }
    }
}
