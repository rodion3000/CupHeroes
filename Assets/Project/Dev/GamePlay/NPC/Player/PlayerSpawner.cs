using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Project.Dev.GamePlay.NPC.Player;
public class PlayerSpawner : MonoBehaviour
{
    private SpineArcher _spineArcher;
    [SerializeField] private Transform playerSpawnPoint;

    [Inject]
    private void Construct(SpineArcher spineArcher)
    {
        _spineArcher = spineArcher;
    }
    
    void Start()
    {
       _spineArcher.transform.position = playerSpawnPoint.position;

    }
    
}
