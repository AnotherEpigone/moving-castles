using Microsoft.Xna.Framework;
using MovingCastles.Components.Serialization;
using MovingCastles.Entities;
using Newtonsoft.Json;
using SadConsole.SerializedTypes;
using System.Linq;
using System.Runtime.Serialization;

namespace MovingCastles.Serialization.Entities
{
    public class DoorJsonConverter : JsonConverter<Door>
    {
        public override void WriteJson(JsonWriter writer, Door value, JsonSerializer serializer) => serializer.Serialize(writer, (DoorSerialized)value);

        public override Door ReadJson(JsonReader reader, System.Type objectType, Door existingValue,
                                        bool hasExistingValue, JsonSerializer serializer) => serializer.Deserialize<DoorSerialized>(reader);
    }

    [DataContract]
    public class DoorSerialized : McEntitySerialized
    {
        [DataMember] public bool IsOpen;

        public static implicit operator DoorSerialized(Door entity)
        {
            var serializedObject = new DoorSerialized()
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
                IsOpen = entity.IsOpen,
                Components = entity.GetGoRogueComponents<ISerializableComponent>()
                                .Select(c => c.GetSerializable())
                                .ToList(),
            };

            if (!entity.Animations.ContainsKey(serializedObject.AnimationName))
            {
                serializedObject.Animations.Add(entity.Animation);
            }

            return serializedObject;
        }

        public static implicit operator Door(DoorSerialized serializedObject)
        {
            var entity = new Door((Point)serializedObject.Position, serializedObject.Font, serializedObject.IsOpen);

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

            entity.IsVisible = serializedObject.IsVisible;
            entity.PositionOffset = serializedObject.PositionOffset;
            entity.UsePixelPositioning = serializedObject.UsePixelPositioning;
            entity.Name = serializedObject.Name;
            entity.DefaultBackground = serializedObject.DefaultBackground;
            entity.DefaultForeground = serializedObject.DefaultForeground;

            foreach (var componentSerialized in serializedObject.Components)
            {
                entity.AddGoRogueComponent(ComponentFactory.Create(componentSerialized));
            }

            return entity;
        }
    }
}
