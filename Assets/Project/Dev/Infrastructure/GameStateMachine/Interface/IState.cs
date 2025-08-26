namespace Project.Dev.Infrastructure.GameStateMachine.Interface
{
    public interface IState: IExitableState
    {
        void Enter();
    }
}

