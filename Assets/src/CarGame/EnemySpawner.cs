using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarEnemySpawner : MonoBehaviour
{
    public GameObject enemyMissilePrefab;
    public Transform playerTarget;
    public float spawnInterval = 3f;
    public float spawnRadius = 30f;

    private float timer;
    private bool gameRunning = false;

    // Keep track of spawned enemies to clean them up
    private List<GameObject> activeEnemies = new List<GameObject>();

    // private void Start()
    // {
    //     StartGame();
    //}

    void Update()
    {
        if (!gameRunning) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemyCar();
            timer = 0f;
        }
    }

    void SpawnEnemyCar()
    {
        if (playerTarget == null)
        {
            return;
        }

        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = new Vector3(
            playerTarget.position.x + randomCircle.x,
            playerTarget.position.y,
            playerTarget.position.z + randomCircle.y
        );

        GameObject missile = Instantiate(enemyMissilePrefab, spawnPos, Quaternion.identity);
        activeEnemies.Add(missile);

        EnemyCarFollow followScript = missile.GetComponent<EnemyCarFollow>();
        if (followScript)
            followScript.SetTarget(playerTarget);
    }

    public void StartGame(Transform playerCar)
    {
        gameRunning = true;
        timer = 0f;
        playerTarget = playerCar;
    }

    public void EndGame()
    {
        playerTarget = null;
        gameRunning = false;

        // Destroy all currently spawned enemies
        foreach (var enemy in activeEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }

        activeEnemies.Clear();
    }
}