using GoRogue;
using GoRogue.GameFramework;
using Microsoft.Xna.Framework.Input;
using MovingCastles.Components;
using MovingCastles.Consoles;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Maps;
using SadConsole;
using System.Collections.Generic;
using System.Linq;

namespace MovingCastles.GameSystems
{
    public enum State
    {
        AwaitingInput,
        Processing
    }

    public class TurnBasedGame : ITurnBasedGame
    {
        private static readonly Dictionary<Keys, Direction> MovementDirectionMapping = new Dictionary<Keys, Direction>
        {
            { Keys.NumPad7, Direction.UP_LEFT },
            { Keys.NumPad8, Direction.UP },
            { Keys.NumPad9, Direction.UP_RIGHT },
            { Keys.NumPad4, Direction.LEFT },
            { Keys.NumPad6, Direction.RIGHT },
            { Keys.NumPad1, Direction.DOWN_LEFT },
            { Keys.NumPad2, Direction.DOWN },
            { Keys.NumPad3, Direction.DOWN_RIGHT },
            { Keys.Up, Direction.UP },
            { Keys.Down, Direction.DOWN },
            { Keys.Left, Direction.LEFT },
            { Keys.Right, Direction.RIGHT }
        };

        private readonly ILogManager _logManager;

        private Player _player;

        public TurnBasedGame(
            ILogManager logManager)
        {
            _logManager = logManager;
        }

        public MovingCastlesMap Map { get; set; }

        public bool HandleAsPlayerInput(SadConsole.Input.Keyboard info)
        {
            foreach (Keys key in MovementDirectionMapping.Keys)
            {
                if (info.IsKeyPressed(key))
                {
                    _player.Move(MovementDirectionMapping[key]);
                    //_logManager.DebugLog("AI decided to do nothing.");
                    return true;
                }
            }

            return false;
        }

        public void RegisterPlayer(Player player)
        {
            _player = player;
            RegisterEntity(player);
        }

        public void RegisterEntity(McEntity entity)
        {
            entity.Moved += Entity_Moved;
            entity.Bumped += Entity_Bumped;
        }

        public void UnregisterEntity(McEntity entity)
        {
            entity.Moved -= Entity_Moved;
            entity.Bumped -= Entity_Bumped;
        }

        private void Entity_Bumped(object sender, ItemMovedEventArgs<McEntity> e)
        {
            _logManager.DebugLog($"{e.Item.Name} bumped into something.");
            var meleeAttackComponent = e.Item.GetGoRogueComponent<IMeleeAttackerComponent>();
            if (meleeAttackComponent != null)
            {
                var healthComponent = Map.GetEntities<BasicEntity>(e.NewPosition)
                    .SelectMany(e =>
                    {
                        if (!(e is IHasComponents entity))
                        {
                            return new IHealthComponent[0];
                        }

                        return entity.GetComponents<IHealthComponent>();
                    })
                    .FirstOrDefault();
                if (healthComponent != null)
                {
                    var damage = meleeAttackComponent.GetDamage();
                    healthComponent.ApplyDamage(damage);

                    var targetName = (healthComponent.Parent as BasicEntity)?.Name ?? "something";
                    _logManager.EventLog($"{e.Item.Name} hit {targetName} for {damage:F0} damage.");

                    if (healthComponent.Dead)
                    {
                        _logManager.EventLog($"{targetName} was slain.");
                        Map.RemoveEntity(healthComponent.Parent);
                    }
                }
            }
        }

        private void Entity_Moved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            if (!(e.Item is BasicEntity movingEntity))
            {
                return;
            }

            if (movingEntity == _player)
            {
                Map.CalculateFOV(_player.Position, _player.FOVRadius, Radius.SQUARE);
            }

            var stepTriggers = Map.GetEntities<BasicEntity>(movingEntity.Position)
                .SelectMany(e =>
                {
                    if (!(e is IHasComponents entity))
                    {
                        return new IStepTriggeredComponent[0];
                    }

                    return entity.GetComponents<IStepTriggeredComponent>();
                });

            foreach (var trigger in stepTriggers)
            {
                trigger.OnStep(movingEntity);
            }
        }
    }
}
