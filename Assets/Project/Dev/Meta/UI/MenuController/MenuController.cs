using System.Threading.Tasks;
using CustomExtensions.Tasks;
using Project.Data.StageData;
using Project.Dev.Infrastructure.GameStateMachine.States;
using Project.Dev.Services.Logging;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Dev.Meta.UI.MenuController
{
    public class MenuController: MonoBehaviour
    {
        [SerializeField] private StageLocalData _stageLocalData;
        [SerializeField] private Button startButton;
        private GameStateMachine _stateMachine;
        private ILoggingService _logger;

        [Inject]
        private void Construct(GameStateMachine stateMachine, ILoggingService logger)
        {
            _stateMachine = stateMachine;
            _logger = logger;
        }

        public async Task Initialize()
        {
            await UnityTaskExtensions.UnitySynchronizationContext;
            StartLvlButtonClicked();
            _logger.LogMessage("Initializing", this);
        }

        private void StartLvlButtonClicked()
        {
            startButton.onClick.AddListener((() => 
            {
                if (_stageLocalData != null)
                {
                    _stateMachine.Enter<LoadLevelState, StageLocalData>(_stageLocalData);
                }
                else
                {
                    _logger.LogMessage("Не выбран уровень!");
                }
                
            }));
        }
        
    }
}
