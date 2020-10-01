namespace MovingCastles.Ui
{
    public interface IMenuProvider
    {
        InventoryWindow Inventory { get; }

        DeathWindow Death { get; }
    }
}