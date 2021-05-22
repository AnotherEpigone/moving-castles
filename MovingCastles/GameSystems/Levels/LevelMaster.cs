using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using System;

namespace MovingCastles.GameSystems.Levels
{
    public class LevelMaster : ILevelMaster
    {
        private readonly IStructureFactory _structureFactory;
        private readonly IGameModeMaster _gameModeMaster;

        public LevelMaster(
            IStructureFactory structureFactory,
            IGameModeMaster gameModeMaster)
        {
            _structureFactory = structureFactory;
            _gameModeMaster = gameModeMaster;
        }

        public event EventHandler LevelChanging;
        public event EventHandler LevelChanged;

        public Level Level { get; set; }

        public Structure Structure { get; set; }

        public void ChangeLevel(string targetMapId, SpawnConditions spawnConditions, Wizard player, ILogManager logManager)
        {
            LevelChanging?.Invoke(this, EventArgs.Empty);

            Level = Structure.GetLevel(targetMapId, player, spawnConditions);

            logManager.StoryLog($"Entered {Level.Name}.");
            LevelChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ChangeStructure(string structureId, string targetMapId, SpawnConditions spawnConditions, Wizard player, ILogManager logManager)
        {
            if (structureId == Structure.Id)
            {
                return;
            }

            LevelChanging?.Invoke(this, EventArgs.Empty);

            Structure = _structureFactory.CreateById(structureId, _gameModeMaster);

            if (Structure.Mode != _gameModeMaster.Mode)
            {
                _gameModeMaster.SetGameMode(
                    Structure.Mode,
                    () => Level = Structure.GetLevel(targetMapId, player, spawnConditions));
            }
            else
            {
                Level = Structure.GetLevel(targetMapId, player, spawnConditions);
                LevelChanged?.Invoke(this, EventArgs.Empty);
            }

            logManager.StoryLog($"Entered {Level.Name}.");
        }
    }
}
