using System;
using System.Html;
using Numbers.Web.ViewModels;
using Numbers.Web.Views;

namespace Numbers.Web
{
    public class Application : IGameHost
    {
        private const int LevelMargins = 5;
        private const int DefaultLevel = 50;

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
                Game = GameFactory.CreateFromSolutionRange(Level - LevelMargins, Level + LevelMargins);
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
                    Level = (int)Math.Min((double)Level * 1.2, 100);
                }

                if (levelChange == LevelChange.Harder)
                {
                    Level = (int)Math.Max((double)Level * 0.8, LevelMargins);
                }
            }

            Game = GameFactory.CreateFromSolutionRange(Level - LevelMargins, Level + LevelMargins);
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

            gameView.Left = Math.Max(0, (Window.InnerWidth - GameView.Width) / 2);
            gameView.Top = Math.Max(0, (Window.InnerHeight - GameView.Height) / 2);

            Document.Body.Style.Width = String.Format("{0}px", Window.InnerWidth);
            Document.Body.Style.Height = String.Format("{0}px", Window.InnerHeight);
        }

        public static void Main()
        {
            Application application = new Application();
            Window.AddEventListener("load", e => application.Run());
        }
    }
}
