using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private GameObject _playerPrefab;
    private CarController _currentPlayer;

    public CarController SpawnPlayer()
    {
        if (_currentPlayer == null)
        { 
            var playerObj = Instantiate(_playerPrefab, _spawnPosition.position, Quaternion.identity);
            _currentPlayer = playerObj.GetComponent<CarController>();
        }
        return _currentPlayer;
    }
    
    public CarController GetPlayer()
    {
        return _currentPlayer;
    }
}
