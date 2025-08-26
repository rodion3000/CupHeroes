using Project.Dev.GamePlay.Logic;
using UnityEngine;
using Zenject;

public class GamePlayInstaller : MonoInstaller
{
    [SerializeField] private LevelProgressWather levelProgressWather;
    public override void InstallBindings()
    {
        Container.BindInstance(levelProgressWather);
    }


    
}
