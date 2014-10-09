using System;
using System.Html;
using Numbers.Web.ViewModels;
using Numbers.Web.Views;
using Numbers.Web.Controls;
using System.Text.RegularExpressions;

namespace Numbers.Web
{
    public class Application : IGameHost
    {
        private const int EasiestLevel = 0;
        private const int DefaultLevel = 20;
        private const int HardestLevel = 100;

        private const string GameLevelConfigurationKey = "GameLevel";
        private const string GameHashConfigurationKey = "GameHash";

        private int gameLevel;
        private int GameLevel
        {
            get { return gameLevel; }
            set
            {
                gameLevel = value;
                configuration.SetValue(GameLevelConfigurationKey, value.ToString());
            }
        }

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

        private static readonly Regex GameHashRegex = new Regex("^([0-9]+-)+[0-9]+$");
        private static readonly Regex SetLevelRegex = new Regex("^Level=([0-9]+)$", "i");

        private IConfiguration configuration;
        private DialogContainer dialogContainer;
        private bool customGame;
        private GameView gameView;
        private ToolsView toolsView;
        private Statistics statistics;
        private bool firstTime;

        public Application()
        {
            //
        }

        public void Run()
        {
            configuration = new Configuration();
            dialogContainer = new DialogContainer();

            statistics = new Statistics(configuration);
            statistics.ReportSessionStart();

            toolsView = new ToolsView(dialogContainer, statistics);

            int storedGameLevel;
            if (Int32.TryParse(configuration.GetValue(GameLevelConfigurationKey), out storedGameLevel))
            {
                gameLevel = storedGameLevel;
            }
            else
            {
                gameLevel = DefaultLevel;
                firstTime = true;
            }

            Document.Body.AppendChild(toolsView.HtmlElement);
            Document.Body.AppendChild(dialogContainer.HtmlElement);

            Window.AddEventListener("hashchange", e => OnHashChanged());
            Window.AddEventListener("resize", e => UpdateLayout());
            Window.AddEventListener("unload", e => statistics.ReportSessionEnd());

            CreateInitialGame();
        }

        private void CreateInitialGame()
        {
            string hash = Window.Location.Hash.TrimStart('#');

            if (GameHashRegex.Exec(hash) != null)
            {
                customGame = true;
                Game = GameFactory.CreateFromHash(hash);
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
                    Game = GameFactory.CreateFromLevel(GameLevel);
                }

                customGame = false;
            }
        }

        public void NewGame()
        {
            statistics.ReportGameEnd();

            if (!customGame && !firstTime && Game.StepsCount > 0)
            {
                if (!Game.IsSolved || Game.HintsCount > 3)
                {
                    GameLevel = Math.Max(GameLevel - Math.Max((100 - GameLevel) / 10, 1), EasiestLevel);
                }
                else if (Game.HintsCount == 0 && Game.StepsCount < 20)
                {
                    GameLevel = Math.Min(GameLevel + Math.Max((100 - GameLevel) / 10, 1), HardestLevel);
                }
            }

            firstTime = firstTime && customGame;
            customGame = false;
            Game = GameFactory.CreateFromLevel(GameLevel);
        }

        private void OnHashChanged()
        {
            string hash = Window.Location.Hash.TrimStart('#');
            RegexMatch match;

            match = GameHashRegex.Exec(hash);
            if (match != null)
            {
                if (Game.ToString() != hash)
                {
                    customGame = true;
                    Game = GameFactory.CreateFromHash(hash);
                }

                return;
            }

            match = SetLevelRegex.Exec(hash);
            if (match != null)
            {
                GameLevel = Math.Min(Math.Max(Int32.Parse(match[1]), EasiestLevel), HardestLevel);
                Console.WriteLine(String.Format("Level changed to {0}", GameLevel));
                return;
            }

            Console.WriteLine(String.Format("Can't parse hash \"{0}\"", hash));
        }

        private void OnGameChanged()
        {
            if (gameView != null)
            {
                Document.Body.RemoveChild(gameView.HtmlElement);
                gameView.Dispose();
            }

            if (Game.ToString() != Window.Location.Hash.TrimStart('#'))
            {
                Window.History.ReplaceState(null, Document.Title, Window.Location.Href.Substring(0, Window.Location.Href.IndexOf("#")));
            }

            configuration.SetValue(GameHashConfigurationKey, Game.ToString());

            statistics.ReportGameStart(Game);
            Game.Solved += (sender, e) => statistics.ReportGameEnd();

            GameViewModel gameViewModel = new GameViewModel(Game, this);
            gameView = new GameView(gameViewModel, firstTime && !customGame);
            toolsView.GameHash = Game.ToString();
            UpdateLayout();

            Document.Body.AppendChild(gameView.HtmlElement);

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
