using MovingCastles.Entities;
using MovingCastles.GameSystems.Factions;
using MovingCastles.GameSystems.Levels;
using MovingCastles.GameSystems.Time;
using MovingCastles.Ui.Windows;

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
            ITimeMaster timeMaster)
        {
            var levelMaster = new LevelMaster(structureFactory, gameModeMaster)
            {
                Level = level,
                Structure = structure,
            };

            var factionMaster = new FactionMaster();
            var storyWindow = new StoryActionWindow();

            return new DungeonMaster(
                player,
                levelMaster,
                factionMaster,
                timeMaster,
                gameModeMaster,
                storyWindow);
        }

        public IDungeonMaster Create(
            Wizard player,
            Level level,
            Structure structure,
            IGameModeMaster gameModeMaster,
            IStructureFactory structureFactory)
            => Create(
                player,
                level,
                structure,
                gameModeMaster,
                structureFactory,
                new TimeMaster());
    }
}
