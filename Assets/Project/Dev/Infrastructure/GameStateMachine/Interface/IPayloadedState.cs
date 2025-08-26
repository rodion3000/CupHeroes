
namespace Project.Dev.Infrastructure.GameStateMachine.Interface
{
    public interface IPayloadedState<TPayload> : IExitableState
    {
        void Enter(TPayload payload);
    }
}

