using GoRogue;
using Microsoft.Xna.Framework;
using MovingCastles.Components;
using MovingCastles.Components.ItemComponents;
using MovingCastles.Components.Stats;
using MovingCastles.GameSystems.Factions;
using MovingCastles.GameSystems.Items;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Player;
using MovingCastles.GameSystems.Spells;
using MovingCastles.Ui;
using SadConsole;
using System.Linq;

namespace MovingCastles.Entities
{
    public class EntityFactory : IEntityFactory
    {
        private readonly Font _font;
        private readonly ILogManager _logManager;

        public EntityFactory(Font font, ILogManager logManager)
        {
            _font = font;
            _logManager = logManager;
        }

        public McEntity CreateActor(Coord position, ActorTemplate actorTemplate)
        {
            var actor = new McEntity(
                actorTemplate.Id,
                actorTemplate.Name,
                Color.White,
                Color.Transparent,
                actorTemplate.Glyph,
                position,
                (int)Maps.DungeonMapLayer.ACTORS,
                isWalkable: false,
                isTransparent: true,
                actorTemplate.NameColor,
                actorTemplate.FactionName,
                System.Guid.NewGuid());

            actorTemplate.CreateComponents().ForEach(c => actor.AddGoRogueComponent(c));
            actor.AddGoRogueComponent(new SummaryControlComponent());

            // workaround Entity construction bugs by setting font afterward
            actor.Font = _font;
            actor.OnCalculateRenderPosition();

            foreach (var subTileTemplate in actorTemplate.SubTiles)
            {
                actor.SubTiles.Add(CreateSubTile(actor, subTileTemplate));
            }

            return actor;
        }

        public McEntity CreateSubTile(McEntity parent, SubTileTemplate template)
        {
            var subTile = new McEntity(
                parent.TemplateId,
                parent.Name,
                Color.White,
                Color.Transparent,
                template.Glyph,
                parent.Position + template.Offset,
                (int)Maps.DungeonMapLayer.ACTORS,
                isWalkable: false,
                isTransparent: true,
                parent.NameColor,
                parent.FactionName,
                parent.UniqueId)
            {
                Anchor = parent,
            };

            // workaround Entity construction bugs by setting font afterward
            subTile.Font = _font;
            subTile.OnCalculateRenderPosition();

            return subTile;
        }

        public McEntity CreateItem(Coord position, ItemTemplate itemTemplate)
        {
            var item = new McEntity(
                    itemTemplate.Id,
                    itemTemplate.Name,
                    Color.White,
                    Color.Transparent,
                    itemTemplate.Glyph,
                    position,
                    (int)Maps.DungeonMapLayer.ITEMS,
                    isWalkable: true,
                    isTransparent: true,
                    ColorHelper.ItemGrey,
                    Faction.None,
                    System.Guid.NewGuid());
            item.AddGoRogueComponent(new SummaryControlComponent());
            item.AddGoRogueComponent(new PickupItemTemplateComponent(itemTemplate));

            // workaround Entity construction bugs by setting font afterward
            item.Font = _font;
            item.OnCalculateRenderPosition();

            return item;
        }

        public McEntity CreateItem(Coord position, Item item)
        {
            var mapItem = new McEntity(
                    item.TemplateId,
                    item.Name,
                    Color.White,
                    Color.Transparent,
                    item.Glyph,
                    position,
                    (int)Maps.DungeonMapLayer.ITEMS,
                    isWalkable: true,
                    isTransparent: true,
                    ColorHelper.ItemGrey,
                    Faction.None,
                    System.Guid.NewGuid());
            mapItem.AddGoRogueComponent(new SummaryControlComponent());
            mapItem.AddGoRogueComponent(new PickupItemComponent(item));

            // workaround Entity construction bugs by setting font afterward
            mapItem.Font = _font;
            mapItem.OnCalculateRenderPosition();

            return mapItem;
        }

        public McEntity CreateDoodad(Coord position, DoodadTemplate template)
        {
            var doodad = new McEntity(
                    template.Id,
                    template.Name,
                    Color.White,
                    Color.Transparent,
                    template.Glyph,
                    position,
                    (int)Maps.DungeonMapLayer.DOODADS,
                    isWalkable: template.Walkable,
                    isTransparent: template.Transparent,
                    template.NameColor,
                    Faction.None,
                    System.Guid.NewGuid());
            doodad.AddGoRogueComponent(new SummaryControlComponent());

            // workaround Entity construction bugs by setting font afterward
            doodad.Font = _font;
            doodad.OnCalculateRenderPosition();

            foreach (var subTileTemplate in template.SubTiles)
            {
                doodad.SubTiles.Add(CreateSubTile(doodad, subTileTemplate));
            }

            return doodad;
        }

        public McEntity CreateDoor(Coord position)
        {
            var door = new Door(position, _font);

            door.AddGoRogueComponent(new OpenDoorComponent());
            door.AddGoRogueComponent(new UseDoorComponent());

            return door;
        }

        public Wizard CreatePlayer(Coord position, WizardTemplate playerInfo)
        {
            var wizard = new Wizard(position, playerInfo, _font);

            wizard.AddGoRogueComponent(new MeleeAttackerComponent(1));
            wizard.AddGoRogueComponent(new SpellCastingComponent(
                SpellAtlas.ConjureFlame,
                SpellAtlas.RayOfFrost,
                SpellAtlas.EtherealStep,
                SpellAtlas.Haste));
            wizard.AddGoRogueComponent(new HealthComponent(playerInfo.MaxHealth, playerInfo.Health, 0));
            wizard.AddGoRogueComponent(new EndowmentPoolComponent(playerInfo.MaxEndowment, playerInfo.Endowment, 0.5f));
            wizard.AddGoRogueComponent(new InventoryComponent(5, playerInfo.Items.Select(i => Item.FromTemplate(i)).ToArray()));
            wizard.AddGoRogueComponent(new ActorStatComponent(1f, 1f, 1f));
            wizard.AddGoRogueComponent(new EquipmentComponent(new EquipCategory[]
            {
                new EquipCategory(EquipCategoryId.Staff, "Staff", 1),
                new EquipCategory(EquipCategoryId.Weapon, "Weapon", 1),
                new EquipCategory(EquipCategoryId.Cloak, "Cloak", 1),
                new EquipCategory(EquipCategoryId.Trinket, "Trinket", 1),
            }));

            return wizard;
        }
    }
}
