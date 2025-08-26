using Project.Dev.GamePlay.Logic;

namespace Project.Dev.Services.LevelProgress
{
    public class LevelProgressService : ILevelProgressService
    {
        public LevelProgressWather levelProgressWather { get; set; }

        public void InitForLvl(LevelProgressWather lvlController) =>
            levelProgressWather = lvlController;

    }
}
