using System.Threading.Tasks;
using Project.Data.StageData;
using Project.Dev.Infrastructure.Factories.Interfaces;
using Project.Dev.Infrastructure.GameStateMachine.Interface;
using CustomExtensions.Tasks;
using Project.Data.HeroData.StageProgressData;
using Project.Dev.Infrastructure.SceneManagment;
using UnityEngine;

namespace Project.Dev.Infrastructure.GameStateMachine.States
{
    public class LoadLevelState : IPayloadedState<StageLocalData>
    {
        private readonly IHeroFactorie _heroFactory;
        private readonly IStageFactorie _stageFactorie;
        private readonly IUIFactorie _uiFactorie;
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        
        private StageLocalData _stageLocalData;
        private StageProgressData _stageProgressData;

        public LoadLevelState(GameStateMachine gameStateMachine,
            IHeroFactorie heroFactory,
            IStageFactorie stageFactorie,
            IUIFactorie uiFactorie,
            SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _heroFactory = heroFactory;
            _uiFactorie = uiFactorie;
            _stageFactorie = stageFactorie;
            _sceneLoader = sceneLoader;
        }
        
        public void Enter(StageLocalData data)
        {
            _stageLocalData = data;
            _stageProgressData = new StageProgressData();
            _ = WarmUpAndLoad().ProcessErrors();
        }
        
        public void Exit()
        {
            _stageFactorie.CleanUp(_stageLocalData.locationName);
            _stageLocalData = null;
        }

        private async Task WarmUpAndLoad()
        {
            await _heroFactory.WarmUp();
            await _stageFactorie.WarmUp(_stageLocalData.locationName);
            
            var sceneInstance = await _sceneLoader.Load(SceneName.Core);

            await InitUIRoot();
            await InitGameWorld();
            await InitUI();
            _gameStateMachine.Enter<GameLoopState>();
        }

        private async Task InitUIRoot()
        {
            await _uiFactorie.CreateUiRoot();
        }

        private async Task InitUI()
        {
            var hudController = await _uiFactorie.CreateHud();

            hudController.Initialize();
        }

        private async Task InitGameWorld()
        {
            await SetupLocation();
           _stageProgressData.Hero = await SetupHero();
            SetupCamera(_stageProgressData.Hero);
        }
        private void SetupCamera(GameObject hero)
        {
            //set up camera follow
        }

        private async Task SetupLocation() =>
            await _stageFactorie.CreateLocation(_stageLocalData.locationName);
        private async Task<GameObject> SetupHero() =>
           await _heroFactory.Create(_stageLocalData.playerSpawnPoint);
    }
}

