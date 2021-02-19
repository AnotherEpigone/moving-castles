using GoRogue.GameFramework.Components;
using MovingCastles.Components.AiComponents;
using MovingCastles.Components.Effects;
using MovingCastles.Components.Levels;
using MovingCastles.Components.Stats;
using MovingCastles.Components.StoryComponents;
using MovingCastles.Serialization;
using System;
using System.Collections.Generic;

namespace MovingCastles.Components.Serialization
{
    public static class ComponentFactory
    {
        private static readonly Dictionary<string, Func<string, IGameObjectComponent>> _constructors;

        static ComponentFactory()
        {
            _constructors = new Dictionary<string, Func<string, IGameObjectComponent>>()
            {
                { nameof(PickupableComponent), s => new PickupableComponent(new SerializedObject() { Value = s }) },
                { nameof(OpenDoorComponent), _ => new OpenDoorComponent() },
                { nameof(UseDoorComponent), _ => new UseDoorComponent() },
                { nameof(SummaryControlComponent), _ => new SummaryControlComponent() },
                { nameof(SpellCastingComponent), s => new SpellCastingComponent(new SerializedObject() { Value = s }) },
                { nameof(RangedAttackerComponent), s => new RangedAttackerComponent(new SerializedObject() { Value = s }) },
                { nameof(MeleeAttackerComponent), s => new MeleeAttackerComponent(new SerializedObject() { Value = s }) },
                { nameof(HealthComponent), s => new HealthComponent(new SerializedObject() { Value = s }) },
                { nameof(ActorStatComponent), s => new ActorStatComponent(new SerializedObject() { Value = s }) },
                { nameof(WalkAtPlayerAiComponent), s => new WalkAtPlayerAiComponent(new SerializedObject() { Value = s }) },
                { nameof(RangedAttackAiComponent), _ => new RangedAttackAiComponent() },
                { nameof(RandomWalkAiComponent), _ => new RandomWalkAiComponent() },
                { nameof(LinearCompositeAiComponent), s => new LinearCompositeAiComponent(new SerializedObject() { Value = s }) },
                { nameof(InventoryComponent), s => new InventoryComponent(new SerializedObject() { Value = s }) },
                { nameof(StoryMessageComponent), s => new StoryMessageComponent(new SerializedObject() { Value = s }) },
                { nameof(ChangeLevelComponent), s => new ChangeLevelComponent(new SerializedObject() { Value = s }) },
                { nameof(EndowmentPoolComponent), s => new EndowmentPoolComponent(new SerializedObject() { Value = s }) },

                // Effects
                { nameof(BurningTimedEffect), s => new BurningTimedEffect(new SerializedObject() { Value = s }) },
            };
        }

        public static IGameObjectComponent Create(ComponentSerializable serialized)
        {
            if (!_constructors.TryGetValue(serialized.Id, out var constructor))
            {
                throw new NotSupportedException($"Unsupported component id: {serialized.Id}");
            }

            return constructor(serialized.State);
        }
    }
}
