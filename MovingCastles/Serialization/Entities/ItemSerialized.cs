using MovingCastles.Components.Serialization;
using MovingCastles.GameSystems.Items;
using Newtonsoft.Json;
using SadConsole.SerializedTypes;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace MovingCastles.Serialization.Entities
{
    public class ItemJsonConverter : JsonConverter<Item>
    {
        public override void WriteJson(JsonWriter writer, Item value, JsonSerializer serializer) => serializer.Serialize(writer, (ItemSerialized)value);

        public override Item ReadJson(JsonReader reader, Type objectType, Item existingValue,
                                        bool hasExistingValue, JsonSerializer serializer) => serializer.Deserialize<ItemSerialized>(reader);
    }

    [DataContract]
    public class ItemSerialized : McEntitySerialized
    {
        [DataMember] public string Description;

        public static implicit operator ItemSerialized(Item entity)
        {
            var serializedObject = new ItemSerialized()
            {
                AnimationName = entity.Animation != null ? entity.Animation.Name : "",
                Animations = entity.Animations.Values.Select(a => (AnimatedConsoleSerialized)a).ToList(),
                Name = entity.Name,
                NameColor = entity.NameColor,
                Layer = entity.Layer,
                IsWalkable = entity.IsWalkable,
                IsTransparent = entity.IsTransparent,
                IsStatic = entity.IsStatic,
                DefaultBackground = entity.DefaultBackground,
                DefaultForeground = entity.DefaultForeground,
                Font = entity.Font,
                Components = entity.GetGoRogueComponents<ISerializableComponent>()
                                .Select(c => c.GetSerializable())
                                .ToList(),
                Id = entity.UniqueId,
                TemplateId = entity.TemplateId,
                Description = entity.Description,
            };

            if (!entity.Animations.ContainsKey(serializedObject.AnimationName))
            {
                serializedObject.Animations.Add(entity.Animation);
            }

            return serializedObject;
        }

        public static implicit operator Item(ItemSerialized serialized)
        {
            var entity = new Item(
                serialized.TemplateId,
                serialized.Name,
                0,
                serialized.NameColor,
                serialized.Id,
                serialized.Description);

            entity.Animations.Clear();
            foreach (AnimatedConsoleSerialized item in serialized.Animations)
            {
                entity.Animations[item.Name] = item;
            }

            if (entity.Animations.ContainsKey(serialized.AnimationName))
            {
                entity.Animation = entity.Animations[serialized.AnimationName];
            }
            else
            {
                entity.Animation = serialized.Animations[0];
            }

            foreach (var componentSerialized in serialized.Components)
            {
                entity.AddGoRogueComponent(ComponentFactory.Create(componentSerialized));
            }

            return entity;
        }
    }
}
