using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.Components.AiComponents;
using MovingCastles.Components.Stats;
using MovingCastles.Fonts;
using MovingCastles.GameSystems.Factions;
using MovingCastles.Ui;
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
            nameColor: ColorHelper.EnemyName,
            createComponents: () => new List<object>
            {
                new HealthComponent(10),
                new ActorStatComponent(1.2f),
                new MeleeAttackerComponent(5),
                new LinearCompositeAiComponent(
                    new WalkAtPlayerAiComponent(6),
                    new RandomWalkAiComponent()),
            },
            Faction.Goblins);

        public static ActorTemplate GoblinArcher => new ActorTemplate(
            id: "ACTOR_GOBLIN_ARCHER",
            name: "Goblin archer",
            glyph: SpriteAtlas.GoblinArcher,
            nameColor: ColorHelper.EnemyName,
            createComponents: () => new List<object>
            {
                new HealthComponent(10),
                new ActorStatComponent(1f),
                new RangedAttackerComponent(5, 4),
                new LinearCompositeAiComponent(
                    new RangedAttackAiComponent(),
                    new WalkAtPlayerAiComponent(6),
                    new RandomWalkAiComponent()),
            },
            Faction.Goblins);

        public static ActorTemplate Warg => new ActorTemplate(
            id: "ACTOR_WARG",
            name: "Warg",
            glyph: SpriteAtlas.Warg,
            nameColor: ColorHelper.EnemyName,
            createComponents: () => new List<object>
            {
                new HealthComponent(10),
                new ActorStatComponent(2f),
                new MeleeAttackerComponent(5),
                new LinearCompositeAiComponent(
                    new WalkAtPlayerAiComponent(6),
                    new RandomWalkAiComponent()),
            },
            Faction.Goblins);
    }
}
