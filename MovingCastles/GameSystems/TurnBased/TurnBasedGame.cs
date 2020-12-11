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

namespace MovingCastles.GameSystems.TurnBased
{
    public enum State
    {
        PlayerTurn,
        Processing,
        Targetting,
        InteractTargetting,
    }

    public class TurnBasedGame : ITurnBasedGame
    {
        public static readonly Dictionary<Keys, Direction> MovementDirectionMapping = new Dictionary<Keys, Direction>
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
        private readonly IDungeonMaster _dungeonMaster;

        private Wizard _player;

        public TurnBasedGame(ILogManager logManager, IDungeonMaster dungeonMaster)
        {
            _logManager = logManager;
            TargetInteractables = new List<Coord>();

            State = State.PlayerTurn;
            _dungeonMaster = dungeonMaster;
        }

        public DungeonMap Map { get; set; }

        public State State { get; set; }

        public SpellTemplate TargettingSpell { get; private set; }

        public List<Coord> TargetInteractables { get; }

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
                StartInteractTargetting();
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

        public bool HandleAsInteractTargettingInput(SadConsole.Input.Keyboard info)
        {
            if (info.IsKeyPressed(Keys.NumPad5)
                || info.IsKeyPressed(Keys.Z)
                || info.IsKeyPressed(Keys.OemPeriod))
            {
                if (TargetInteractables.Contains(_player.Position))
                {
                    InteractTargetSelected(_player.Position);
                    return true;
                }

                _logManager.EventLog("Nothing to interact with there.");
                return false;
            }

            if (info.IsKeyPressed(Keys.E)
                || info.IsKeyPressed(Keys.Enter))
            {
                if (TargetInteractables.Count == 1)
                {
                    InteractTargetSelected(TargetInteractables[0]);
                    return true;
                }

                _logManager.EventLog("What do you want to interact with?");
                return false;
            }

            foreach (Keys key in MovementDirectionMapping.Keys)
            {
                if (info.IsKeyPressed(key))
                {
                    var targetPos = _player.Position + MovementDirectionMapping[key];
                    if (TargetInteractables.Contains(targetPos))
                    {
                        InteractTargetSelected(targetPos);
                        return true;
                    }

                    _logManager.EventLog("Nothing to interact with there.");
                    return false;
                }
            }

            return false;
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

        public void SpellTargetSelected(Coord mapCoord)
        {
            foreach (var effect in TargettingSpell.Effects)
            {
                effect.Apply(_player, TargettingSpell, Map, mapCoord, _logManager);
            }

            TargettingSpell = null;
            ProcessTurn();
        }

        public void StartSpellTargetting(SpellTemplate spell)
        {
            TargettingSpell = spell;
            State = State.Targetting;
        }

        public void InteractTargetSelected(Coord mapCoord)
        {
            var components = Map.GetEntities<McEntity>(mapCoord)
                    .SelectMany(e => e.GetGoRogueComponents<IInteractTriggeredComponent>());
            components.First().Interact(_player, _logManager, _dungeonMaster);

            TargetInteractables.Clear();
            ProcessTurn();
        }

        private void StartInteractTargetting()
        {
            TargetInteractables.Clear();
            foreach (var point in AdjacencyRule.EIGHT_WAY
                .Neighbors(_player.Position)
                .Concat(_player.Position.Yield()))
            {
                if (Map.GetEntities<McEntity>(point)
                    .Any(e => e.HasGoRogueComponents(typeof(IInteractTriggeredComponent))))
                {
                    TargetInteractables.Add(point);
                }
            }

            if (TargetInteractables.Count == 0)
            {
                _logManager.EventLog("Nothing to interact with.");
                return;
            }

            State = State.InteractTargetting;
        }

        private void ProcessTurn()
        {
            Map.CalculateFOV(_player.Position, _player.FovRadius, Radius.SQUARE);

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
                Map.CalculateFOV(_player.Position, _player.FovRadius, Radius.SQUARE);
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
                trigger.OnStep(movingEntity, _logManager, _dungeonMaster);
            }
        }
    }
}
