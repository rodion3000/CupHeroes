

using Project.Dev.GamePlay.Logic;

namespace Project.Dev.Services.LevelProgress
{
    public interface ILevelProgressService
    {
        LevelProgressWather levelProgressWather { get; set; }

        void InitForLvl(LevelProgressWather lvlController);

    }
}
