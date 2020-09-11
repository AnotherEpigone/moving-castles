namespace MovingCastles.GameSystems.Items
{
    public class InventoryItem : IInventoryItem
    {
        public InventoryItem(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
