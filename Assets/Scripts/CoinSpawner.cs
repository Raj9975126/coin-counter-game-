using System.Collections;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject coinPrefab;
    public float spawnInterval = 1.2f;
    public float xRange = 7.5f;
    public float spawnY = 6.0f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (GameManager.Instance != null && GameManager.Instance.isGameOver)
                yield break;

            yield return new WaitForSeconds(spawnInterval);

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
