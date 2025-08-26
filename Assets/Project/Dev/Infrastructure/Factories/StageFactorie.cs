using System.Threading.Tasks;
using Project.Dev.GamePlay.Location;
using Project.Dev.Infrastructure.AssetManager;
using CustomExtensions.Functional;
using Project.Dev.Infrastructure.Factories.Interfaces;
using Project.Dev.Services.StaticDataService;
using UnityEngine;
using Zenject;

namespace Project.Dev.Infrastructure.Factories
{
    public class StageFactorie : IStageFactorie
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private readonly DiContainer _container;

        public StageFactorie(IAssetProvider assetProvider, DiContainer container, IStaticDataService staticDataService)
        {
            _assetProvider = assetProvider;
            _container = container;
            _staticDataService = staticDataService;
        }
        public async Task WarmUp(string locationName)
        {
            await _assetProvider.Load<GameObject>(key: locationName);
        }

        public void CleanUp(string locationName)
        {
            _assetProvider.Release(key: locationName);
        }

        public async Task<LocationManager> CreateLocation(string locationName)
        {

            var prefab = await _assetProvider.Load<GameObject>(key: locationName);
            return Object.Instantiate(prefab)
                .GetComponent<LocationManager>()
                .With(location => _container.Inject(location))
                .With(location => location._stageLocalData = _staticDataService.SetStageLocalData(location._stageLocalData));
        }
    }
}
