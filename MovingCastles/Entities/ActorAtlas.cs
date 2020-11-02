using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.Components.AiComponents;
using MovingCastles.Fonts;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MovingCastles.Entities
{
    public static class ActorAtlas
    {
        static ActorAtlas()
        {
            ActorsById = typeof(ActorAtlas)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(p => p.GetValue(null))
                .OfType<ActorTemplate>()
                .ToDictionary(
                i => i.Id,
                i => i);
        }

        public static Dictionary<string, ActorTemplate> ActorsById { get; }

        public static ActorTemplate Goblin => new ActorTemplate(
            id: "ACTOR_GOBLIN",
            name: "Goblin",
            glyph: SpriteAtlas.Goblin,
            nameColor: Color.DarkGreen,
            createComponents: () => new List<object>
            {
                new HealthComponent(10),
                new ActorStatComponent(1),
                new MeleeAttackerComponent(5),
                new LinearCompositeAiComponent(
                    new WalkAtPlayerAiComponent(6),
                    new RandomWalkAiComponent()),
            });

        public static ActorTemplate GoblinArcher => new ActorTemplate(
            id: "ACTOR_GOBLIN_ARCHER",
            name: "Goblin archer",
            glyph: SpriteAtlas.GoblinArcher,
            nameColor: Color.DarkGreen,
            createComponents: () => new List<object>
            {
                new HealthComponent(10),
                new ActorStatComponent(1),
                new RangedAttackerComponent(5, 4),
                new LinearCompositeAiComponent(
                    new RangedAttackAiComponent(),
                    new WalkAtPlayerAiComponent(6),
                    new RandomWalkAiComponent()),
            });

        public static ActorTemplate Warg => new ActorTemplate(
            id: "ACTOR_WARG",
            name: "Warg",
            glyph: SpriteAtlas.Warg,
            nameColor: Color.DarkSlateGray,
            createComponents: () => new List<object>
            {
                new HealthComponent(10),
                new ActorStatComponent(2),
                new MeleeAttackerComponent(5),
                new LinearCompositeAiComponent(
                    new WalkAtPlayerAiComponent(6),
                    new RandomWalkAiComponent()),
            });
    }
}
