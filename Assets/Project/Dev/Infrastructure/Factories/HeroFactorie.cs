using System.Threading.Tasks;
using JetBrains.Annotations;
using Project.Dev.Infrastructure.Factories.Interfaces;
using UnityEngine;
using CustomExtensions.Functional;
using Project.Dev.GamePlay.NPC.Player;
using Project.Dev.Infrastructure.AssetManager;
using Project.Dev.Services.StaticDataService;
using Unity.Mathematics;
using Zenject;

namespace Project.Dev.Infrastructure.Factories
{
    public class HeroFactorie : IHeroFactorie
    {
        private const string HeroPrefabId = "Player";
        private readonly IStaticDataService _staticDataService;
        private readonly IAssetProvider _assetProvider;
        private readonly DiContainer _container;

        [CanBeNull] public GameObject Hero { get; private set; }

        public HeroFactorie(IStaticDataService staticDataService, IAssetProvider assetProvider, DiContainer container)
        {
            _staticDataService = staticDataService;
            _assetProvider = assetProvider;
            _container = container;
        }

        public async Task WarmUp()
        {
           await _assetProvider.Load<GameObject>(key: HeroPrefabId);
        }

        public void CleanUp()
        {
            Hero = null;
            _assetProvider.Release(key: HeroPrefabId);
        }

        public async Task<GameObject> Create(Vector3 at)
        {
            var config = _staticDataService.ForHero();
            var prefab = await _assetProvider.Load<GameObject>(key: HeroPrefabId);

            return Hero = Object.Instantiate(prefab, at, quaternion.identity)
                .With(hero => _container.InjectGameObject(hero))
                .With(hero => hero.GetComponent<SpineArcher>()
                     .With(spineArcher =>  spineArcher.arrowSpeed = config.arrowSpeed)
                     .With(spineArcher =>  spineArcher.tiltSpeed = config.tiltSpeed)
                     .With(spineArcher =>  spineArcher.maxTiltAngle = config.maxTiltAngle)
                );
        }
    }

}
