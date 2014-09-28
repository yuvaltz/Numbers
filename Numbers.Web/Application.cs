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

        public Application()
        {
            //
        }

        public void Run()
        {
            configuration = new Configuration();

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
                Game = GameFactory.CreateFromSolutionRange(Level, (int)((Level + 3) * 1.1));
                customGame = false;
            }

            Window.AddEventListener("resize", e => UpdateLayout());
        }

        public void NewGame(LevelChange levelChange)
        {
            if (!customGame)
            {
                if (levelChange == LevelChange.Easier)
                {
                    Level = (int)Math.Min((double)Level * 1.1, EasiestLevel);
                }

                if (levelChange == LevelChange.Harder)
                {
                    Level = (int)Math.Max((double)Level * 0.9, HardestLevel);
                }
            }

            Game = GameFactory.CreateFromSolutionRange(Level, (int)((Level + 3) * 1.1));
            customGame = false;
        }

        public void RestorePreviousGame()
        {
            //
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
            Window.History.ReplaceState(null, Document.Title, String.Format("#{0}", Game));

            GameViewModel gameViewModel = new GameViewModel(Game, this);
            gameView = new GameView(gameViewModel);
            UpdateLayout();

            while (Document.Body.Children.Length > 0)
            {
                Document.Body.RemoveChild(Document.Body.LastChild);
            }

            Document.Body.AppendChild(gameView.HtmlElement);

            gameView.Run();
        }

        private void UpdateLayout()
        {
            if (gameView == null)
            {
                return;
            }

            int viewContainerWidth = 0;
            int viewContainerHeight = 0;

            GetContianerDimension(Window.InnerWidth, Window.InnerHeight, GameView.Width, GameView.Height, ref viewContainerWidth, ref viewContainerHeight);

            gameView.Left = (viewContainerWidth - GameView.Width) / 2;
            gameView.Top = (viewContainerHeight - GameView.Height) / 2;

            Document.Body.Style.Width = String.Format("{0}px", viewContainerWidth);
            Document.Body.Style.Height = String.Format("{0}px", viewContainerHeight);
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
