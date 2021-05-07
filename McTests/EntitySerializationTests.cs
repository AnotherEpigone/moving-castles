using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Items;
using MovingCastles.GameSystems.Player;
using Newtonsoft.Json;
using NUnit.Framework;
using System;

namespace NUnitTestProject1
{
    public class EntitySerializationTests
    {
        [OneTimeTearDown]
        public void TearDown()
        {
            SadConsole.Game.Instance.Exit();
        }

        [OneTimeSetUp]
        public void Setup()
        {
            SadConsole.Game.Create(1, 1);
            SadConsole.Game.Instance.RunOneFrame();
        }

        [Test]
        public void ItemSerialization()
        {
            const string templateId = "template id";
            const string name = "name";
            const int glyph = 1337;
            var id = Guid.Parse("ce5e4220-cf05-4c43-9aa7-4b9e789b47b5");
            const string desc = "description";
            var item = new Item(
                templateId,
                name,
                glyph,
                Color.Red,
                id,
                desc);
            var serialized = JsonConvert.SerializeObject(item);
            var deserialized = JsonConvert.DeserializeObject<Item>(serialized);

            Assert.AreEqual(name, deserialized.Name);
        }

        [Test]
        public void WizardSerialization()
        {
            var template = new WizardTemplate();
            var item = new Wizard(
                Coord.NONE,
                new WizardTemplate(),
                SadConsole.Global.FontDefault);
            var serialized = JsonConvert.SerializeObject(item);
            var deserialized = JsonConvert.DeserializeObject<Wizard>(serialized);

            Assert.AreEqual(template.Name, deserialized.Name);
        }
    }
}