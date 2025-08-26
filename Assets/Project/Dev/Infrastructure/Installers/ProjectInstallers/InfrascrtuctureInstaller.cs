using Project.Data.HeroLocalData;
using Project.Dev.Infrastructure.AssetManager;
using Project.Dev.Infrastructure.Factories;
using Project.Dev.Infrastructure.Factories.Interfaces;
using Project.Dev.Infrastructure.SceneManagment;
using Project.Dev.Services.LevelProgress;
using Project.Dev.Services.Logging;
using Project.Dev.Services.StaticDataService;
using UnityEngine;
using Zenject;

namespace Project.Dev.Infrastructure.Installers.ProjectInstallers
{
    public class InfrascrtuctureInstaller : MonoInstaller
    {

        [SerializeField] private HeroLocalData _heroLocalData;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AddressableProvider>().AsSingle();
            Container.Bind<SceneLoader>().AsSingle();
            BindServices();
            BindFactories();
        }

        private void BindServices()
        {
            Container.Bind<ILoggingService>().To<LoggingService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<StaticDataService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LevelProgressServiceResolver>()
                .AsSingle()
                .CopyIntoDirectSubContainers();
            Container.BindInterfacesAndSelfTo<LevelProgressService>().AsSingle().NonLazy();
        }

        private void BindFactories()
        {
            Container.Bind<HeroLocalData>().FromInstance(_heroLocalData);
            Container.BindInterfacesAndSelfTo<StateFactories>().AsSingle();
            Container.Bind<IHeroFactorie>().To<HeroFactorie>().AsSingle();
            Container.Bind<IStageFactorie>().To<StageFactorie>().AsSingle();
            Container.Bind<IUIFactorie>().To<UIFactorie>().AsSingle();
        }
    }
}
