using GoRogue;
using GoRogue.GameFramework;
using Microsoft.Xna.Framework.Input;
using MovingCastles.Components;
using MovingCastles.Components.AiComponents;
using MovingCastles.Components.Effects;
using MovingCastles.Components.Stats;
using MovingCastles.Components.Triggers;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Combat;
using MovingCastles.GameSystems.Logging;
using MovingCastles.GameSystems.Movement;
using MovingCastles.GameSystems.Spells;
using MovingCastles.GameSystems.Time;
using MovingCastles.GameSystems.Time.Nodes;
using MovingCastles.Maps;
using MovingCastles.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using Troschuetz.Random;
using Troschuetz.Random.Generators;

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
        private readonly IGenerator _rng;
        private readonly Dictionary<System.Guid, McEntity> _registeredEntities;

        private Wizard _player;
        private bool _disposed;

        // we don't want to grab a key that was pressed on a previous map!
        private bool _keyDebounced;

        public TurnBasedGame(ILogManager logManager, IDungeonMaster dungeonMaster)
        {
            _logManager = logManager;
            TargetInteractables = new List<Coord>();

            State = State.PlayerTurn;
            _dungeonMaster = dungeonMaster;
            _keyDebounced = false;

            TargetableTiles = new List<Coord>();

            _rng = new StandardGenerator();

            _registeredEntities = new Dictionary<System.Guid, McEntity>();
        }

        public McMap Map { get; private set; }

        public State State { get; set; }

        public SpellTemplate TargettingSpell { get; private set; }

        public List<Coord> TargetableTiles { get; }

        public List<Coord> TargetInteractables { get; }

        public void Init(McMap map)
        {
            Map = map;

            var secondMarkerNode = new SecondMarkerNode((_dungeonMaster.TimeMaster.JourneyTime.Seconds + 1) * 100);

            _dungeonMaster.TimeMaster.ClearNodes();
            _dungeonMaster.TimeMaster.Enqueue(secondMarkerNode);
        }

        public bool HandleAsPlayerInput(SadConsole.Input.Keyboard info)
        {
            if (info.IsKeyPressed(Keys.NumPad5)
                || info.IsKeyPressed(Keys.Z)
                || info.IsKeyPressed(Keys.OemPeriod))
            {
                if (!_keyDebounced)
                {
                    return true;
                }

                ProcessTurn(TimeHelper.Wait);
                return true;
            }

            if (info.IsKeyPressed(Keys.E)
                || info.IsKeyPressed(Keys.Enter))
            {
                if (!_keyDebounced)
                {
                    return true;
                }

                StartInteractTargetting();
                return true;
            }

            foreach (Keys key in MovementDirectionMapping.Keys)
            {
                if (info.IsKeyPressed(key))
                {
                    if (!_keyDebounced)
                    {
                        return true;
                    }

                    _player.Move(MovementDirectionMapping[key]);

                    ProcessTurn(TimeHelper.GetWalkTime(_player));
                    return true;
                }
            }

            if (!_keyDebounced)
            {
                _keyDebounced = true;
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

                _logManager.StoryLog("Nothing to interact with there.");
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

                _logManager.CombatLog("What do you want to interact with?");
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

                    _logManager.CombatLog("Nothing to interact with there.");
                    return false;
                }
            }

            return false;
        }

        public void RegisterPlayer(Wizard player)
        {
            _player = player;

            // Note, the player doesn't get a turn node enqueued yet, because the
            // game always starts in the playerturn state!
            _player.Moved += Entity_Moved;
            _player.Bumped += Entity_Bumped;
            _player.RemovedFromMap += (_, __) => UnregisterEntity(_player);
        }

        public void RegisterEntity(McEntity entity)
        {
            if (entity.IsSubTile)
            {
                return;
            }

            if (entity.HasGoRogueComponent<IAiComponent>()
                && !_dungeonMaster.TimeMaster.Nodes.Any(node =>
                    node is EntityTurnNode entityNode
                    && entityNode.EntityId == entity.UniqueId))
            {
                var time = _dungeonMaster.TimeMaster.JourneyTime.Ticks + 10;
                var entityTurnNode = new EntityTurnNode(time, entity.UniqueId);
                _dungeonMaster.TimeMaster.Enqueue(entityTurnNode);
            }

            _registeredEntities.Add(entity.UniqueId, entity);
            entity.Moved += Entity_Moved;
            entity.Bumped += Entity_Bumped;
            entity.RemovedFromMap += (_, __) => UnregisterEntity(entity);
        }

        public void UnregisterEntity(McEntity entity)
        {
            if (entity.IsSubTile)
            {
                return;
            }

            _registeredEntities.Remove(entity.UniqueId);
            entity.Moved -= Entity_Moved;
            entity.Bumped -= Entity_Bumped;
        }

        public void SpellTargetSelected(Coord mapCoord)
        {
            HitResult hitResult;
            if (TargettingSpell.TargettingStyle.CanMiss)
            {
                var target = Map.GetActor(mapCoord);
                if (target == null)
                {
                    return;
                }

                hitResult = _dungeonMaster.HitMan.Get(_player, target, _rng);
            }
            else
            {
                hitResult = HitResult.Hit;
            }

            _logManager.CombatLog($"{_player.ColoredName} cast {TargettingSpell.Name}.", true);

            var endowmentComponent = _player.GetGoRogueComponent<IEndowmentPoolComponent>();
            endowmentComponent.ApplyDrain(TargettingSpell.EndowmentCost);

            foreach (var effect in TargettingSpell.Effects)
            {
                effect.Apply(_dungeonMaster, _player, TargettingSpell, Map, hitResult, mapCoord, _logManager);
            }

            var spell = TargettingSpell;
            TargettingSpell = null;

            ProcessTurn(TimeHelper.GetCastTime(_player, spell));
        }

        public void StartSpellTargetting(SpellTemplate spell)
        {
            TargettingSpell = spell;
            State = State.Targetting;

            TargetableTiles.Clear();
            foreach (var tile in Map.FOV.CurrentFOV)
            {
                var (targetable, actualCoord) = Map.GetTarget(_player.Position, tile, spell.TargettingStyle);
                if (targetable && !TargetableTiles.Contains(actualCoord))
                {
                    TargetableTiles.Add(actualCoord);
                }
            }
        }

        public void InteractTargetSelected(Coord mapCoord)
        {
            var components = Map.GetEntities<McEntity>(mapCoord)
                    .SelectMany(e => e.GetGoRogueComponents<IInteractTriggeredComponent>());
            components.First().Interact(_player, _logManager, _dungeonMaster);

            TargetInteractables.Clear();
            ProcessTurn(TimeHelper.Interact);
        }

        private void ProcessAiTurn(System.Guid id, long time)
        {
            if (!_registeredEntities.TryGetValue(id, out var entity)
                || !entity.HasMap)
            {
                return;
            }

            var ai = entity.GetGoRogueComponent<IAiComponent>();
            var (success, ticks) = ai?.Run(Map, _rng, _dungeonMaster, _logManager) ?? (false, -1);
            if (!success || ticks < 1)
            {
                return;
            }

            var nextTurnNode = new EntityTurnNode(time + ticks, entity.UniqueId);
            _dungeonMaster.TimeMaster.Enqueue(nextTurnNode);
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
                _logManager.CombatLog("Nothing to interact with.");
                return;
            }

            State = State.InteractTargetting;
        }

        private void ProcessTurn(int playerTurnTicks)
        {
            if (_disposed)
            {
                return;
            }

            var playerTurnNode = new WizardTurnNode(
                _dungeonMaster.TimeMaster.JourneyTime.Ticks + playerTurnTicks);
            _dungeonMaster.TimeMaster.Enqueue(playerTurnNode);

            Map.CalculateFOV(_player.Position, _player.FovRadius, Radius.SQUARE);

            State = State.Processing;
            var node = _dungeonMaster.TimeMaster.Dequeue();
            while (node is not WizardTurnNode)
            {
                if (!_player.HasMap)
                {
                    break;
                }

                switch (node)
                {
                    case EntityTurnNode entityTurnNode:
                        ProcessAiTurn(
                            entityTurnNode.EntityId,
                            _dungeonMaster.TimeMaster.JourneyTime.Ticks);
                        break;
                    case SecondMarkerNode:
                        ProcessSecondMarker(_dungeonMaster.TimeMaster.JourneyTime);
                        break;
                    default:
                        throw new System.NotSupportedException($"Unhandled time master node type: {node.GetType()}");
                }

                node = _dungeonMaster.TimeMaster.Dequeue();
            }
            State = State.PlayerTurn;
        }

        private void ProcessSecondMarker(McTimeSpan time)
        {
            foreach (var entity in _registeredEntities.Values.Append(_player))
            {
                var effects = entity.GetGoRogueComponents<ITimedEffect>();
                foreach (var effect in effects.ToArray())
                {
                    effect.OnTick(time, _logManager, _dungeonMaster, _rng);
                }
            }

            _player.GetGoRogueComponent<IEndowmentPoolComponent>()?.ApplyRegen();
            _player.GetGoRogueComponent<IHealthComponent>()?.ApplyRegen();

            var secondMarkerNode = new SecondMarkerNode(time.Ticks + 100);
            _dungeonMaster.TimeMaster.Enqueue(secondMarkerNode);
        }

        private void MeleeAttack(
            McEntity attacker,
            McEntity defender,
            IMeleeAttackerComponent meleeAttackComponent,
            IHealthComponent healthComponent)
        {
            var hitResult = _dungeonMaster.HitMan.Get(attacker, defender, _rng);
            var damage = meleeAttackComponent.GetDamage(_rng);
            var targetName = defender.ColoredName;
            switch (hitResult)
            {
                case HitResult.Hit:
                    _logManager.CombatLog($"{attacker.ColoredName} {ColorHelper.GetParserString("hit", ColorHelper.ImportantAction)} {targetName} for {damage:F0} damage.", attacker is Wizard);
                    healthComponent.ApplyDamage(damage, _logManager);
                    break;
                case HitResult.Glance:
                    damage /= 4;
                    _logManager.CombatLog($"{attacker.ColoredName} hit {targetName} with a {ColorHelper.GetParserString("glancing blow", ColorHelper.ImportantAction)} for {damage:F0} damage.", attacker is Wizard);
                    healthComponent.ApplyDamage(damage, _logManager);
                    break;
                case HitResult.Miss:
                    _logManager.CombatLog($"{attacker.ColoredName} {ColorHelper.GetParserString("missed", ColorHelper.ImportantAction)} {targetName}.", attacker is Wizard);
                    break;
                case HitResult.Crit:
                    damage *= 2;
                    _logManager.CombatLog($"{attacker.ColoredName} hit {targetName} with a {ColorHelper.GetParserString("critical blow", ColorHelper.ImportantAction)} for {damage:F0} damage.", attacker is Wizard);
                    healthComponent.ApplyDamage(damage, _logManager);
                    break;
            }
        }

        private void Entity_Bumped(object sender, EntityBumpedEventArgs e)
        {
            var bumpTriggeredComponent = Map.GetEntities<McEntity>(e.BumpedPosition)
                    .SelectMany(e =>
                    {
                        if (!(e is McEntity entity))
                        {
                            return System.Array.Empty<IBumpTriggeredComponent>();
                        }

                        return entity.GetGoRogueComponents<IBumpTriggeredComponent>();
                    })
                    .FirstOrDefault();
            bumpTriggeredComponent?.Bump(e.BumpingEntity, _logManager, _dungeonMaster, _rng);

            var meleeAttackComponent = e.BumpingEntity.GetGoRogueComponent<IMeleeAttackerComponent>();
            var attacker = meleeAttackComponent?.Parent as McEntity;
            if (attacker != null)
            {
                var healthComponent = Map.GetEntities<McEntity>(e.BumpedPosition)
                    .SelectMany(e =>
                    {
                        if (!(e is McEntity entity))
                        {
                            return System.Array.Empty<IHealthComponent>();
                        }

                        return entity.GetGoRogueComponents<IHealthComponent>();
                    })
                    .FirstOrDefault();
                var defender = healthComponent?.Parent as McEntity;
                if (defender != null
                    && _dungeonMaster.FactionMaster.AreEnemies(attacker.FactionName, defender.FactionName))
                {
                    e.MadeMeleeAttack = true;
                    MeleeAttack(e.BumpingEntity, defender, meleeAttackComponent, healthComponent);
                }
            }
        }

        private void Entity_Moved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            if (e.Item is not McEntity movingEntity)
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
                    if (!(e is McEntity entity))
                    {
                        return System.Array.Empty<IStepTriggeredComponent>();
                    }

                    return entity.GetGoRogueComponents<IStepTriggeredComponent>();
                });

            foreach (var trigger in stepTriggers)
            {
                trigger.OnStep(movingEntity, _logManager, _dungeonMaster, _rng);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    UnregisterEntity(_player);

                    foreach (var entity in _registeredEntities.Values)
                    {
                        UnregisterEntity(entity);
                    }
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
