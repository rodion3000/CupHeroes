using System.Collections.Generic;
using Project.Data.HeroLocalData;
using Project.Data.StageData;

namespace Project.Dev.Services.StaticDataService
{
    public interface IStaticDataService
    {
        public StageLocalData SetStageLocalData(StageLocalData localData);
        public StageLocalData GetStageLocalData();
        StageLocalData ForStage(string stageKey);
        List<StageLocalData> GetAllStages { get; }
        public HeroLocalData ForHero();
    }
}
