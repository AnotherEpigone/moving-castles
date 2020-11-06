using GoRogue;
using GoRogue.GameFramework;
using Microsoft.Xna.Framework.Input;
using MovingCastles.Components;
using MovingCastles.Components.AiComponents;
using MovingCastles.Components.Triggers;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Spells;
using MovingCastles.Maps;
using System.Collections.Generic;
using System.Linq;

namespace MovingCastles.GameSystems.TurnBasedGame
{
    public enum State
    {
        PlayerTurn,
        Processing,
        Targetting
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

        private Wizard _player;

        public TurnBasedGame(
            ILogManager logManager)
        {
            _logManager = logManager;

            State = State.PlayerTurn;
        }

        public DungeonMap Map { get; set; }

        public State State { get; set; }

        public SpellTemplate TargettingSpell { get; private set; }

        public bool HandleAsPlayerInput(SadConsole.Input.Keyboard info)
        {
            if (info.IsKeyPressed(Keys.NumPad5)
                || info.IsKeyPressed(Keys.Z)
                || info.IsKeyPressed(Keys.OemPeriod))
            {
                _player.GetGoRogueComponent<IHealthComponent>()?.ApplyBaseRegen();

                ProcessTurn();
                return true;
            }

            if (info.IsKeyPressed(Keys.E)
                || info.IsKeyPressed(Keys.Enter))
            {
                Interact();
                ProcessTurn();
                return true;
            }

            foreach (Keys key in MovementDirectionMapping.Keys)
            {
                if (info.IsKeyPressed(key))
                {
                    _player.Move(MovementDirectionMapping[key]);

                    ProcessTurn();
                    return true;
                }
            }

            return false;
        }

        private void Interact()
        {
            var components = new List<IInteractTriggeredComponent>();
            var points = AdjacencyRule.EIGHT_WAY.Neighbors(_player.Position);
            foreach (var point in points)
            {
                components.AddRange(Map.GetEntities<McEntity>(point)
                    .SelectMany(e => e.GetGoRogueComponents<IInteractTriggeredComponent>()));
            }

            // TODO select which thing to interact with...
            if (components.Count == 0)
            {
                return;
            }

            components.First().Interact(_player);
        }

        public void RegisterPlayer(Wizard player)
        {
            _player = player;
            RegisterEntity(player);
        }

        public void RegisterEntity(McEntity entity)
        {
            entity.Moved += Entity_Moved;
            entity.Bumped += Entity_Bumped;
            entity.RemovedFromMap += (_, __) => UnregisterEntity(entity);
        }

        public void UnregisterEntity(McEntity entity)
        {
            entity.Moved -= Entity_Moved;
            entity.Bumped -= Entity_Bumped;
        }

        public void TargetSelected(Coord mapCoord)
        {
            foreach (var effect in TargettingSpell.Effects)
            {
                effect.Apply(_player, Map, mapCoord, _logManager);
            }

            TargettingSpell = null;
            ProcessTurn();
        }

        public void StartTargetting(SpellTemplate spell)
        {
            TargettingSpell = spell;
            State = State.Targetting;
        }

        private void ProcessTurn()
        {
            Map.CalculateFOV(_player.Position, _player.FOVRadius, Radius.SQUARE);

            State = State.Processing;
            foreach (var entity in Map.Entities.Items.OfType<McEntity>().ToList())
            {
                if (!_player.HasMap)
                {
                    break;
                }

                if (entity.CurrentMap != Map)
                {
                    continue;
                }

                var ai = entity.GetGoRogueComponent<IAiComponent>();
                ai?.Run(Map, _logManager);
            }
            State = State.PlayerTurn;
        }

        private void Entity_Bumped(object sender, ItemMovedEventArgs<McEntity> e)
        {
            var bumpTriggeredComponent = Map.GetEntities<McEntity>(e.NewPosition)
                    .SelectMany(e =>
                    {
                        if (!(e is IHasComponents entity))
                        {
                            return new IBumpTriggeredComponent[0];
                        }

                        return entity.GetComponents<IBumpTriggeredComponent>();
                    })
                    .FirstOrDefault();
            bumpTriggeredComponent?.Bump(e.Item);

            var meleeAttackComponent = e.Item.GetGoRogueComponent<IMeleeAttackerComponent>();
            if (meleeAttackComponent != null)
            {
                var healthComponent = Map.GetEntities<McEntity>(e.NewPosition)
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
                    healthComponent.ApplyDamage(damage, _logManager);

                    var targetName = (healthComponent.Parent as McEntity)?.ColoredName ?? "something";
                    _logManager.EventLog($"{e.Item.ColoredName} hit {targetName} for {damage:F0} damage.");
                }
            }
        }

        private void Entity_Moved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            if (!(e.Item is McEntity movingEntity))
            {
                return;
            }

            if (movingEntity == _player)
            {
                Map.CalculateFOV(_player.Position, _player.FOVRadius, Radius.SQUARE);
            }

            var stepTriggers = Map.GetEntities<McEntity>(movingEntity.Position)
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
