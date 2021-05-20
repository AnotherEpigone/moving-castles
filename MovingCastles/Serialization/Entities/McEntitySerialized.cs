using Microsoft.Xna.Framework;
using MovingCastles.Components.Serialization;
using MovingCastles.Entities;
using Newtonsoft.Json;
using SadConsole.SerializedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MovingCastles.Serialization.Entities
{
    public class McEntityJsonConverter : JsonConverter<McEntity>
    {
        public override void WriteJson(JsonWriter writer, McEntity value, JsonSerializer serializer) => serializer.Serialize(writer, (McEntitySerialized)value);

        public override McEntity ReadJson(JsonReader reader, System.Type objectType, McEntity existingValue,
                                        bool hasExistingValue, JsonSerializer serializer) => serializer.Deserialize<McEntitySerialized>(reader);
    }

    [DataContract]
    public class McEntitySerialized : EntitySerialized
    {
        [DataMember] public Color NameColor;
        [DataMember] public int Layer;
        [DataMember] public string TemplateId;
        [DataMember] public bool IsWalkable;
        [DataMember] public bool IsTransparent;
        [DataMember] public bool IsStatic;
        [DataMember] public List<ComponentSerializable> Components;
        [DataMember] public string FactionName;
        [DataMember] public Guid Id;
        [DataMember] public int Glyph;

        public static implicit operator McEntitySerialized(McEntity entity)
        {
            var serializedObject = new McEntitySerialized()
            {
                AnimationName = entity.Animation != null ? entity.Animation.Name : "",
                Animations = entity.Animations.Values.Select(a => (AnimatedConsoleSerialized)a).ToList(),
                IsVisible = entity.IsVisible,
                Position = (Point)entity.Position,
                PositionOffset = entity.PositionOffset,
                UsePixelPositioning = entity.UsePixelPositioning,
                Name = entity.Name,
                NameColor = entity.NameColor,
                Layer = entity.Layer,
                IsWalkable = entity.IsWalkable,
                IsTransparent = entity.IsTransparent,
                IsStatic = entity.IsStatic,
                DefaultBackground = entity.DefaultBackground,
                DefaultForeground = entity.DefaultForeground,
                Font = entity.Font,
                Glyph = entity.Glyph,
                Components = entity.GetGoRogueComponents<ISerializableComponent>()
                                .Select(c => c.GetSerializable())
                                .ToList(),
                TemplateId = entity.TemplateId,
                FactionName = entity.FactionName,
                Id = entity.UniqueId,
            };

            if (!entity.Animations.ContainsKey(serializedObject.AnimationName))
            {
                serializedObject.Animations.Add(entity.Animation);
            }

            return serializedObject;
        }

        public static implicit operator McEntity(McEntitySerialized serializedObject)
        {
            var entity = new McEntity(
                serializedObject.TemplateId,
                serializedObject.Name,
                serializedObject.DefaultForeground,
                serializedObject.DefaultBackground,
                serializedObject.Glyph,
                (Point)serializedObject.Position,
                serializedObject.Layer,
                serializedObject.IsWalkable,
                serializedObject.IsTransparent,
                serializedObject.NameColor,
                serializedObject.FactionName,
                serializedObject.Id)
            {
                Font = serializedObject.Font,
                IsVisible = serializedObject.IsVisible,
                PositionOffset = serializedObject.PositionOffset,
                UsePixelPositioning = serializedObject.UsePixelPositioning,
                Name = serializedObject.Name,
            };

            entity.Animations.Clear();
            foreach (AnimatedConsoleSerialized item in serializedObject.Animations)
            {
                entity.Animations[item.Name] = item;
            }

            if (entity.Animations.ContainsKey(serializedObject.AnimationName))
            {
                entity.Animation = entity.Animations[serializedObject.AnimationName];
            }
            else
            {
                entity.Animation = serializedObject.Animations[0];
            }

            foreach (var componentSerialized in serializedObject.Components)
            {
                entity.AddGoRogueComponent(ComponentFactory.Create(componentSerialized));
            }

            return entity;
        }
    }
}
