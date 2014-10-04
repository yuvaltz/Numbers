using System;
using System.Html;
using Numbers.Web.ViewModels;
using Numbers.Web.Views;

namespace Numbers.Web
{
    public class Application : IGameHost
    {
        private const int EasiestLevel = 120;
        private const int DefaultLevel = 80;
        private const int HardestLevel = 1;

        private const string LevelConfigurationKey = "Level";
        private const string GameHashConfigurationKey = "GameHash";

        private int level;
        private int Level
        {
            get { return level; }
            set
            {
                level = value;
                configuration.SetValue(LevelConfigurationKey, value.ToString());
            }
        }

        private int LevelMinimumSolutions { get { return Level; } }

        private int LevelMaximumSolutions { get { return Level + Math.Max(Level / 10, 3); } }

        private Game game;
        private Game Game
        {
            get { return game; }
            set
            {
                if (game == value)
                {
                    return;
                }

                game = value;
                OnGameChanged();
            }
        }

        private IConfiguration configuration;
        private bool customGame;
        private GameView gameView;
        private ToolsView toolsView;
        private Statistics statistics;

        public Application()
        {
            //
        }

        public void Run()
        {
            configuration = new Configuration();
            statistics = new Statistics(configuration);
            statistics.ReportSessionStart();

            int storedLevel;
            level = Int32.TryParse(configuration.GetValue(LevelConfigurationKey), out storedLevel) ? storedLevel : DefaultLevel;

            Window.AddEventListener("hashchange", e => OnHashChanged());

            if (!String.IsNullOrEmpty(Window.Location.Hash))
            {
                Game = GameFactory.CreateFromHash(Window.Location.Hash.TrimStart('#'));
                customGame = true;
            }
            else
            {
                string lastGameHash = configuration.GetValue(GameHashConfigurationKey);

                if (!String.IsNullOrEmpty(lastGameHash))
                {
                    Game = GameFactory.CreateFromHash(lastGameHash);
                }
                else
                {
                    Game = GameFactory.CreateFromSolutionRange(LevelMinimumSolutions, LevelMaximumSolutions);
                }

                customGame = false;
            }

            Window.AddEventListener("resize", e => UpdateLayout());
            Window.AddEventListener("unload", e => statistics.ReportSessionEnd());
        }

        public void NewGame()
        {
            statistics.ReportGameEnd();

            if (!customGame && Game.StepsCount > 0)
            {
                if (!Game.IsSolved || Game.HintsCount > 3)
                {
                    Level = Math.Min(Level + Math.Max(Level / 10, 1), EasiestLevel);
                }
                else if (Game.HintsCount == 0 && Game.StepsCount < 20)
                {
                    Level = Math.Max(Level - Math.Max(Level / 10, 1), HardestLevel);
                }
            }

            Game = GameFactory.CreateFromSolutionRange(LevelMinimumSolutions, LevelMaximumSolutions);
            customGame = false;
        }

        private void OnHashChanged()
        {
            if (Game.ToString() != Window.Location.Hash.TrimStart('#'))
            {
                Game = GameFactory.CreateFromHash(Window.Location.Hash.TrimStart('#'));
                customGame = true;
            }
        }

        private void OnGameChanged()
        {
            if (gameView != null)
            {
                Document.Body.RemoveChild(gameView.HtmlElement);
                gameView.Dispose();
            }

            if (toolsView != null)
            {
                Document.Body.RemoveChild(toolsView.HtmlElement);
                toolsView.Dispose();
            }

            if (Game.ToString() != Window.Location.Hash.TrimStart('#'))
            {
                Window.History.ReplaceState(null, Document.Title, Window.Location.Href.Substring(0, Window.Location.Href.IndexOf("#")));
            }

            configuration.SetValue(GameHashConfigurationKey, Game.ToString());

            statistics.ReportGameStart(Game);
            Game.Solved += (sender, e) => statistics.ReportGameEnd();

            GameViewModel gameViewModel = new GameViewModel(Game, this);
            gameView = new GameView(gameViewModel);
            toolsView = new ToolsView(Game.ToString());
            UpdateLayout();

            Document.Body.AppendChild(gameView.HtmlElement);
            Document.Body.AppendChild(toolsView.HtmlElement);

            gameView.Run();
        }

        private void UpdateLayout()
        {
            if (gameView == null || toolsView == null)
            {
                return;
            }

            int viewContainerWidth = 0;
            int viewContainerHeight = 0;

            GetContianerDimension(Window.InnerWidth, Window.InnerHeight, GameView.Width, GameView.Height, ref viewContainerWidth, ref viewContainerHeight);

            gameView.Left = (viewContainerWidth - GameView.Width) / 2;
            gameView.Top = (viewContainerHeight - GameView.Height) / 2;

            toolsView.HtmlElement.Style.Visibility = viewContainerWidth < gameView.Left + GameView.Width + ToolsView.Width + 8 && viewContainerHeight < gameView.Top + GameView.Height + ToolsView.Height + 8 ? "collapse" : "visible";

            Document.Body.Style.Width = String.Format("{0}px", viewContainerWidth);
            Document.Body.Style.Height = String.Format("{0}px", viewContainerHeight);

            Window.ScrollTo((viewContainerWidth - Window.InnerWidth) / 2, (viewContainerHeight - Window.InnerHeight) / 2);
        }

        private static void GetContianerDimension(int windowWidth, int windowHeight, int viewWidth, int viewHeight, ref int viewContainerWidth, ref int viewContainerHeight)
        {
            if (windowWidth >= viewWidth && windowHeight >= viewHeight)
            {
                viewContainerWidth = windowWidth;
                viewContainerHeight = windowHeight;
                return;
            }

            double windowSizeRatio = (double)windowWidth / windowHeight;
            double viewSizeRatio = (double)viewWidth / viewHeight;

            if (windowSizeRatio > viewSizeRatio)
            {
                viewContainerWidth = (int)(viewHeight * windowSizeRatio);
                viewContainerHeight = viewHeight;
            }
            else
            {
                viewContainerWidth = viewWidth;
                viewContainerHeight = (int)(viewWidth / windowSizeRatio);
            }
        }

        public static void Main()
        {
            Application application = new Application();
            Window.AddEventListener("load", e => application.Run());
        }
    }
}
