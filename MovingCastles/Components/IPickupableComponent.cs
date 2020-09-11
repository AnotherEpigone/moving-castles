using MovingCastles.GameSystems.Items;
using System.Collections.Generic;

namespace MovingCastles.Components
{
    public interface IPickupableComponent : IStepTriggeredComponent
    {
        List<IInventoryItem> Items { get; }
    }
}