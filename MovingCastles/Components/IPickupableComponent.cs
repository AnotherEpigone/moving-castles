using MovingCastles.Components.Triggers;
using MovingCastles.GameSystems.Items;
using System.Collections.Generic;

namespace MovingCastles.Components
{
    public interface IPickupableComponent : IStepTriggeredComponent
    {
        List<ItemTemplate> Items { get; }
    }
}