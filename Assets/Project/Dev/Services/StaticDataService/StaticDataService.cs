using System.Collections.Generic;
using System.Linq;
using Project.Data.HeroLocalData;
using Project.Data.StageData;

namespace Project.Dev.Services.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<string, StageLocalData> _stages;
        private HeroLocalData _heroLocalData;
        private StageLocalData _staticLocalData;

        public StaticDataService(HeroLocalData heroLocalData)
        {
            _heroLocalData = heroLocalData;
        }

        public StageLocalData ForStage(string stageKey) =>
            _stages.TryGetValue(stageKey, out var stageData)
                ? stageData
                : null;

        public StageLocalData SetStageLocalData(StageLocalData localData)
        {
            _staticLocalData = localData;
            return _staticLocalData;
        }

        public StageLocalData GetStageLocalData()
        {
            return _staticLocalData;
        }

        public List<StageLocalData> GetAllStages =>
            _stages.Values.ToList();

        public HeroLocalData ForHero() =>
            _heroLocalData;

    }
}
