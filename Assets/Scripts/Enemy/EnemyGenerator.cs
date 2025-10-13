using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private float _spawnDelay = 3f;
    [SerializeField] private float _minSpawnHeight = -2f;
    [SerializeField] private float _maxSpawnHeight = 3f;
    [SerializeField] private Enemy _enemyPrefab;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        var wait = new WaitForSeconds(_spawnDelay);

        while (enabled)
        {
            SpawnEnemy();
            yield return wait;
        }
    }

    private void SpawnEnemy()
    {
        float spawnPositionY = Random.Range(_minSpawnHeight, _maxSpawnHeight);
        Vector3 spawnPoint = new Vector3(transform.position.x, spawnPositionY, transform.position.z);

        var enemy = Instantiate(_enemyPrefab, spawnPoint, Quaternion.identity);
    }
}