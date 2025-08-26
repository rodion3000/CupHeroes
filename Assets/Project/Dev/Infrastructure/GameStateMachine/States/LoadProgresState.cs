using System.Threading.Tasks;
using CustomExtensions.Tasks;
using Project.Dev.Infrastructure.GameStateMachine.Interface;

namespace Project.Dev.Infrastructure.GameStateMachine.States
{
    public class LoadProgresState : IState
    {
        private readonly GameStateMachine _stateMachine;

        public LoadProgresState(GameStateMachine gameStateMachine)
        {
            _stateMachine = gameStateMachine;
        }

        public void Enter()
        {

            _ = LoadProgress().ProcessErrors();

        }
        public void Exit()
        {
            
        }

        private async Task LoadProgress()
        {
            await UnityTaskExtensions.UnitySynchronizationContext;
            _stateMachine.Enter<LoadMetaState>();
        }


    }
}

