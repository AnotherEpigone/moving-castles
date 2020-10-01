namespace MovingCastles.Ui
{
    public class MenuProvider : IMenuProvider
    {
        public MenuProvider(
            InventoryWindow inventory,
            DeathWindow death)
        {
            Inventory = inventory;
            Death = death;
        }

        public InventoryWindow Inventory { get; }

        public DeathWindow Death { get; }
    }
}
