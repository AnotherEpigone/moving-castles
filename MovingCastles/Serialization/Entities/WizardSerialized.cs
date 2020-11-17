using Microsoft.Xna.Framework;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Player;
using Newtonsoft.Json;
using SadConsole.SerializedTypes;
using System.Linq;
using System.Runtime.Serialization;

namespace MovingCastles.Serialization.Entities
{
    public class WizardJsonConverter : JsonConverter<Wizard>
    {
        public override void WriteJson(JsonWriter writer, Wizard value, JsonSerializer serializer) => serializer.Serialize(writer, (WizardSerialized)value);

        public override Wizard ReadJson(JsonReader reader, System.Type objectType, Wizard existingValue,
                                        bool hasExistingValue, JsonSerializer serializer) => serializer.Deserialize<WizardSerialized>(reader);
    }

    [DataContract]
    public class WizardSerialized : McEntitySerialized
    {
        public static implicit operator WizardSerialized(Wizard entity)
        {
            var serializedObject = new WizardSerialized()
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
            };

            if (!entity.Animations.ContainsKey(serializedObject.AnimationName))
            {
                serializedObject.Animations.Add(entity.Animation);
            }

            return serializedObject;
        }

        public static implicit operator Wizard(WizardSerialized serializedObject)
        {
            var entity = new Wizard(
                (Point)serializedObject.Position,
                PlayerInfo.CreateDefault(), // TODO
                serializedObject.Font);

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

            return entity;
        }
    }
}
