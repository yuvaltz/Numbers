using System;
namespace Numbers.Web
{
    public enum LevelChange { Easier, Same, Harder }

    public interface IGameHost
    {
        void NewGame(LevelChange levelChange);
        void RestorePreviousGame();
    }
}