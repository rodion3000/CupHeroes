using System.Collections.Generic;
using Project.Dev.Infrastructure.GameStateMachine.Interface;
using Project.Dev.Services.Interfaces;
using System.Threading.Tasks;
using CustomExtensions.Tasks;

namespace Project.Dev.Infrastructure.GameStateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly List<IInitializableAsync> _initializableServices;

        public BootstrapState(GameStateMachine stateMachine, List<IInitializableAsync> initializableServices)
        {
            _stateMachine = stateMachine;
            _initializableServices = initializableServices;
        }

        public void Enter()
        {
            _ = InitializeServices().ProcessErrors();
        }
        public void Exit()
        {

        }

        private async Task InitializeServices()
        {
            foreach (var service in _initializableServices)
                await service.InitializeAsync();

            _stateMachine.Enter<LoadProgresState>();
        }


    }
}

