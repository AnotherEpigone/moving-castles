using GoRogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MovingCastles.Components;
using MovingCastles.Entities;
using MovingCastles.GameSystems.Spells;
using MovingCastles.Maps;
using SadConsole;
using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using XnaRect = Microsoft.Xna.Framework.Rectangle;

namespace MovingCastles.Ui.Consoles
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class CastleMapConsole : ContainerConsole, ITurnBasedGameConsole
    {
        private readonly IMapModeMenuProvider _menuProvider;
        private readonly SadConsole.Console _mouseHighlight;

        private Point _lastSummaryConsolePosition;
        private bool _disposed;

        public event EventHandler<ConsoleListEventArgs> SummaryConsolesChanged;
        public event EventHandler<string> FlavorMessageChanged;

        public McMap Map { get; }

        public ScrollingConsole MapRenderer { get; }

        public Wizard Player { get; }

        public SadConsole.Console ThisConsole => this;

        public CastleMapConsole(
            int width,
            int height,
            Font font,
            IMapModeMenuProvider menuProvider,
            McMap map)
        {
            _menuProvider = menuProvider;

            _mouseHighlight = new SadConsole.Console(1, 1, font);
            _mouseHighlight.SetGlyph(0, 0, 1, ColorHelper.WhiteHighlight);
            _mouseHighlight.UseMouse = false;

            Map = map;

            MapRenderer = Map.CreateRenderer(new XnaRect(0, 0, width, height), font);
            MapRenderer.UseMouse = false;

            IsFocused = true;

            Player = map.Player;

            Map.CalculateFOV(Player.Position, Player.FovRadius, Radius.SQUARE);
            MapRenderer.CenterViewPortOnPoint(Player.Position);

            Children.Add(MapRenderer);
            Children.Add(_mouseHighlight);
        }

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            if (info.IsKeyPressed(Keys.Escape))
            {
                _menuProvider.Pop.Show();
                return true;
            }

            if (info.IsKeyPressed(Keys.I))
            {
                _menuProvider.Inventory.Show(Player.GetGoRogueComponent<IInventoryComponent>());
                return true;
            }

            return base.ProcessKeyboard(info);
        }

        public override bool ProcessMouse(MouseConsoleState state)
        {
            var mapState = new MouseConsoleState(MapRenderer, state.Mouse);

            var mapCoord = new Coord(
                mapState.ConsoleCellPosition.X + MapRenderer.ViewPort.X,
                mapState.ConsoleCellPosition.Y + MapRenderer.ViewPort.Y);

            _mouseHighlight.IsVisible = mapState.IsOnConsole && Map.Explored[mapCoord];
            _mouseHighlight.Position = mapState.ConsoleCellPosition;

            if (mapState.IsOnConsole
                && _lastSummaryConsolePosition != mapState.ConsoleCellPosition
                && Map.FOV.CurrentFOV.Contains(mapCoord))
            {
                // update summaries
                var summaryControls = new List<SadConsole.Console>();
                foreach (var entity in Map.GetEntities<BasicEntity>(mapCoord))
                {
                    var control = entity.GetGoRogueComponent<ISummaryControlComponent>()?.GetSidebarSummary();
                    if (control != null)
                    {
                        summaryControls.Add(control);
                    }
                }

                _lastSummaryConsolePosition = mapState.ConsoleCellPosition;
                // TODO
                // SummaryConsolesChanged?.Invoke(this, new ConsoleListEventArgs(summaryControls));
            }

            if (!_mouseHighlight.IsVisible && _lastSummaryConsolePosition != default)
            {
                // remove the summaries if we just moved out of a valid location
                _lastSummaryConsolePosition = default;
                // TODO
                // SummaryConsolesChanged?.Invoke(this, new ConsoleListEventArgs(new List<Console>()));
            }

            return base.ProcessMouse(state);
        }

        public void StartTargetting(SpellTemplate spell)
        {
            throw new NotImplementedException();
        }

        public void SetMap(McMap map, Font tilesetFont)
        {
            throw new NotImplementedException();
        }

        public void UnsetMap()
        {
            throw new NotImplementedException();
        }

        private string DebuggerDisplay
        {
            get
            {
                return string.Format($"{nameof(CastleMapConsole)} ({Position.X}, {Position.Y})");
            }
        }

        public int ViewportWidth => throw new NotImplementedException();

        public int ViewportHeight => throw new NotImplementedException();

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
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
