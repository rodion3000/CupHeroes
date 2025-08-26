using Project.Data.StageData;
using Project.Dev.Infrastructure.GameStateMachine.States;
using Project.Dev.Services.StaticDataService;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Dev.Meta.UI.HudController
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private Button nextLvlButton;
        [SerializeField] private Button returnButton;
        private GameStateMachine _stateMachine;
        private IStaticDataService _staticData;
        
        [Inject]
        private void Construct(GameStateMachine stateMachine, IStaticDataService staticData)
        {
            _stateMachine = stateMachine;
            _staticData = staticData;
        }

        public void Initialize()
        {
            NextLvlButtonClicked();
            ReturnsButtonClicked();
        }
        private void NextLvlButtonClicked()
        {
            nextLvlButton.onClick.AddListener((() => 
            {
                _stateMachine.Enter<LoadLevelState, StageLocalData>(_staticData.GetStageLocalData());
            }));
        }

        private void ReturnsButtonClicked() =>
            returnButton.onClick.AddListener(() =>
                _stateMachine.Enter<LoadMetaState>());

    }
}
