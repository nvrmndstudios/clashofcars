using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [SerializeField] private CarEnemySpawner _carEnemySpawner;
    [SerializeField] private BarrelSpawner _barrelSpawner;
    [SerializeField] private PlayerSpawner _playerSpawner;
    [SerializeField] private UIController _uiController;
    [SerializeField] private CameraFollow _cameraFollow;

    private void OnWaveStarted(int wave)
    {
        _uiController.UpdateWave(wave);
    }

    public void StartGame()
    {
        var player =  _playerSpawner.SpawnPlayer();
        var playerTransform =  player.transform;
        _cameraFollow.target = playerTransform;
        _barrelSpawner.StartGame(playerTransform);
        _carEnemySpawner.StartGame(playerTransform);
    }

    public void EndGame()
    {
        _cameraFollow.target = null;
        _barrelSpawner.EndGame();
        _carEnemySpawner.EndGame();
    }  
}
