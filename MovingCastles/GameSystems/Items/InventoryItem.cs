namespace MovingCastles.GameSystems.Items
{
    public class InventoryItem : IInventoryItem
    {
        public InventoryItem(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; }
        public string Description { get; }
    }
}
