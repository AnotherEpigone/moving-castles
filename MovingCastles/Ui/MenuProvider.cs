namespace MovingCastles.Ui
{
    public class MenuProvider : IMenuProvider
    {
        public MenuProvider(
            InventoryWindow inventory)
        {
            Inventory = inventory;
        }

        public InventoryWindow Inventory { get; }
    }
}
