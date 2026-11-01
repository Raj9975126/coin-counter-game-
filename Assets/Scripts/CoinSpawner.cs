using System.Collections;
using UnityEngine;

/// <summary>
/// Spawns coins periodically from the top of the screen at random X positions.
/// </summary>
public class CoinSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("Coin prefab to spawn")]
    public GameObject coinPrefab;

    [Tooltip("Interval in seconds between coin spawns")]
    public float spawnInterval = 1.2f;

    [Tooltip("Horizontal range for random spawning (-xRange to +xRange)")]
    public float xRange = 7.5f;

    [Tooltip("Height (Y coordinate) from which coins spawn")]
    public float spawnY = 6.0f;

    private Coroutine spawnCoroutine;

    void Start()
    {
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // Pause spawning if game over
            if (GameManager.Instance != null && GameManager.Instance.isGameOver)
                yield break;

            yield return new WaitForSeconds(spawnInterval);

            // Re-check game over state after wait
            if (GameManager.Instance != null && GameManager.Instance.isGameOver)
                yield break;

            SpawnCoin();
        }
    }

    private void SpawnCoin()
    {
        if (coinPrefab == null) return;

        float randomX = Random.Range(-xRange, xRange);
        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);

        Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
    }
}
