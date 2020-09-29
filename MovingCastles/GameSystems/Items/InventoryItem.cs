namespace MovingCastles.GameSystems.Items
{
    public class InventoryItem : IInventoryItem
    {
        private readonly ItemTemplate _itemTemplate;

        public InventoryItem(ItemTemplate itemTemplate)
        {
            _itemTemplate = itemTemplate;
        }

        public string Name => _itemTemplate.Name;

        public string Description => _itemTemplate.Description;
    }
}
