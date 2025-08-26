using System;
using Zenject;
using Project.Dev.GamePlay.Logic;

namespace Project.Dev.Services.LevelProgress
{
    public class LevelProgressServiceResolver : IInitializable, IDisposable
    {
        private readonly ILevelProgressService _levelProgressService;
        private readonly LevelProgressWather _levelProgressWatcher;

        public LevelProgressServiceResolver(
            ILevelProgressService levelProgressService,
            [Inject(Source = InjectSources.Local, Optional = true)] LevelProgressWather levelProgressWatcher)
        {
            _levelProgressService = levelProgressService;
            _levelProgressWatcher = levelProgressWatcher;
        }

        public void Initialize() =>
            _levelProgressService.InitForLvl(_levelProgressWatcher);

        public void Dispose() =>
            _levelProgressService.InitForLvl(null);

    }
}
