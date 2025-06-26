using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BarrelSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject barrelPrefab;
    public Transform playerTarget;

    [Header("Spawn Radius")]
    public float spawnRadius = 10f;
    public float spawnY = 0f;

    [Header("Spawn Logic")]
    public float respawnDelay = 0.3f;
    
    
    [Header("UI Indicator")]
    public GameObject barrelMarkerPrefab;     // Assign the UI icon prefab
    public Canvas canvas;                     // Main UI canvas
    public Camera worldCamera;  

    private List<GameObject> activeBarrels = new List<GameObject>();
    private Coroutine spawnLoopCoroutine;

    public void StartGame(Transform player)
    {
        playerTarget = player;
        if (spawnLoopCoroutine == null)
            spawnLoopCoroutine = StartCoroutine(SpawnBarrelsLoop());
    }

    public void EndGame()
    {
        playerTarget = null;
        if (spawnLoopCoroutine != null)
        {
            StopCoroutine(spawnLoopCoroutine);
            spawnLoopCoroutine = null;
        }

        foreach (var barrel in activeBarrels)
        {
            if (barrel != null) Destroy(barrel);
        }

        activeBarrels.Clear();
    }

    private IEnumerator SpawnBarrelsLoop()
    {
        while (true)
        {
            yield return new WaitUntil(() => activeBarrels.Count == 0);
            yield return new WaitForSeconds(respawnDelay);

            if (playerTarget == null)
                continue;

            int barrelCount = Random.Range(1, 5); // Spawn 1 or 2 barrels
            for (int i = 0; i < barrelCount; i++)
            {
                Vector3 spawnPos = GetRandomSpawnPositionAroundPlayer();
                GameObject barrel = Instantiate(barrelPrefab, spawnPos, Quaternion.identity);
                activeBarrels.Add(barrel);
                
                Barrel barrelScript = barrel.GetComponent<Barrel>();
                if (barrelScript != null)
                {
                    barrelScript.OnBarrelBlasted += HandleBarrelBlasted;
                }
                
                GameObject marker = Instantiate(barrelMarkerPrefab, canvas.transform);
                BarrelIndicatorController markerScript = marker.GetComponent<BarrelIndicatorController>();

                if (markerScript != null)
                {
                    markerScript.SetTarget(barrelScript, worldCamera); // 👈 Pass the full barrel reference
                }
            }
        }
    }

    private Vector3 GetRandomSpawnPositionAroundPlayer()
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        return new Vector3(
            playerTarget.position.x + randomCircle.x,
            spawnY,
            playerTarget.position.z + randomCircle.y
        );
    }

    private void HandleBarrelBlasted(GameObject barrel)
    {
        activeBarrels.Remove(barrel);
    }
}