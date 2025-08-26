using Project.Dev.Infrastructure.GameStateMachine.States;
using Project.Dev.Services.Logging;
using UnityEngine;
using Zenject;

namespace Project.Dev.GamePlay.Logic
{
    public class LevelProgressWather : MonoBehaviour
    {
        private GameStateMachine _stateMachine;
        private ILoggingService _logger;

        [Inject]
        private void Construct(GameStateMachine stateMachine, ILoggingService logger)
        {
            _stateMachine = stateMachine;
            _logger = logger;
        }

        public void RunLvl()
        {
            _logger.LogMessage("Run Level", this);
        }
    }
}
