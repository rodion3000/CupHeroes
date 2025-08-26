using Project.Dev.Infrastructure.GameStateMachine.States;
using Zenject;

namespace Project.Dev.Infrastructure.Installers.ProjectInstallers
{
    public class GameStateMachineInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<BootstrapState>().AsSingle().NonLazy();
            Container.Bind<LoadProgresState>().AsSingle().NonLazy();
            Container.Bind<LoadMetaState>().AsSingle().NonLazy();
            Container.Bind<LoadLevelState>().AsSingle().NonLazy();
            Container.Bind<GameLoopState>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<GameStateMachine.States.GameStateMachine>().AsSingle();
        }
    }
}

