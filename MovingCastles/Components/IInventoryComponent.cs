using MovingCastles.GameSystems.Items;
using System.Collections.Generic;

namespace MovingCastles.Components
{
    public interface IInventoryComponent
    {
        List<ItemTemplate> Items { get; }
    }
}