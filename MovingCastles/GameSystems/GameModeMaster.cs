using MovingCastles.Entities;
using MovingCastles.GameSystems.Logging;
using MovingCastles.Ui;
using MovingCastles.Ui.Consoles;
using SadConsole;
using System;

namespace MovingCastles.GameSystems
{
    public class GameModeMaster : IGameModeMaster
    {
        private readonly Font _dungeonFont;
        private readonly Font _castleFont;
        private readonly IEntityFactory _dungeonEntityFactory;
        private readonly IEntityFactory _castleEntityFactory;
        private readonly ITurnBasedGameConsoleFactory _dungeonConsoleFactory;
        private readonly ITurnBasedGameConsoleFactory _castleConsoleFactory;

        public GameModeMaster(ILogManager logManager, GameMode mode)
        {
            _dungeonFont = Global.Fonts[UiManager.TilesetFontName].GetFont(Font.FontSizes.One);
            _castleFont = Global.Fonts[UiManager.TilesetFontName].GetFont(Font.FontSizes.Two);
            _dungeonEntityFactory = new EntityFactory(_dungeonFont, logManager);
            _castleEntityFactory = new EntityFactory(_castleFont, logManager);
            _dungeonConsoleFactory = new DungeonMapConsoleFactory();
            _castleConsoleFactory = new DungeonMapConsoleFactory(); // todo castle
            
            Mode = mode;
        }

        public event EventHandler ModeChanged;

        public GameMode Mode { get; private set; }

        public Font Font => Mode switch
	    {
            GameMode.Castle => _castleFont,
            GameMode.Dungeon => _dungeonFont,
            _ => throw new ArgumentException($"Unsupported gamemode: {Mode}"),
	    };

        public IEntityFactory EntityFactory => Mode switch
        {
            GameMode.Castle => _castleEntityFactory,
            GameMode.Dungeon => _dungeonEntityFactory,
            _ => throw new ArgumentException($"Unsupported gamemode: {Mode}"),
        };

        public ITurnBasedGameConsoleFactory GameConsoleFactory => Mode switch
        {
            GameMode.Castle => _castleConsoleFactory,
            GameMode.Dungeon => _dungeonConsoleFactory,
            _ => throw new ArgumentException($"Unsupported gamemode: {Mode}"),
        };

        public void SetGameMode(GameMode gameMode)
        {
            if (Mode == gameMode)
            {
                return;
            }

            Mode = gameMode;
            ModeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
