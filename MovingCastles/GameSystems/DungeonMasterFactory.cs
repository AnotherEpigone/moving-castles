using MovingCastles.Entities;
using MovingCastles.GameSystems.Combat;
using MovingCastles.GameSystems.Factions;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Time;
using MovingCastles.Ui;

namespace MovingCastles.GameSystems
{
    public class DungeonMasterFactory : IDungeonMasterFactory
    {
        public IDungeonMaster Create(
            Wizard player,
            Level level,
            Structure structure,
            IGameModeMaster gameModeMaster,
            IStructureFactory structureFactory,
            ITimeMaster timeMaster,
            IUiManager uiManager)
        {
            var levelMaster = new LevelMaster(structureFactory, gameModeMaster)
            {
                Level = level,
                Structure = structure,
            };

            var factionMaster = new FactionMaster();
            var scenarioMaster = new ScenarioMaster(uiManager);
            var hitMan = new HitMan();

            return new DungeonMaster(
                player,
                levelMaster,
                factionMaster,
                timeMaster,
                gameModeMaster,
                scenarioMaster,
                hitMan);
        }
    }
}
