using Microsoft.Xna.Framework;
using MovingCastles.Components.Serialization;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Journal;
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
        [DataMember] public JournalEntry[] JournalEntries;

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
                Components = entity.GetGoRogueComponents<ISerializableComponent>()
                                .Select(c => c.GetSerializable())
                                .ToList(),
                JournalEntries = entity.JournalEntries.SelectMany(e => e).ToArray(),
                Id = entity.UniqueId,
            };

            if (!entity.Animations.ContainsKey(serializedObject.AnimationName))
            {
                serializedObject.Animations.Add(entity.Animation);
            }

            return serializedObject;
        }

        public static implicit operator Wizard(WizardSerialized serializedObject)
        {
            var playerTemplate = new GameSystems.Player.WizardTemplate()
            {
                JournalEntries = serializedObject.JournalEntries.ToList(),
            };

            var entity = new Wizard(
                (Point)serializedObject.Position,
                playerTemplate, // TODO
                serializedObject.Font,
                serializedObject.Id)
            {
                IsVisible = serializedObject.IsVisible,
                PositionOffset = serializedObject.PositionOffset,
                UsePixelPositioning = serializedObject.UsePixelPositioning,
                Name = serializedObject.Name,
                DefaultBackground = serializedObject.DefaultBackground,
                DefaultForeground = serializedObject.DefaultForeground
            };

            return entity;
        }
    }
}
