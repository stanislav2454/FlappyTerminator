using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private float _spawnDelay = 3f;
    [SerializeField] private float _minSpawnHeight = -2f;
    [SerializeField] private float _maxSpawnHeight = 3f;
    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private EventBus _eventBus;

    private bool _canSpawn = false;

    private void Awake()
    {
        if (_eventBus == null)
            Debug.LogError("Компонент \"EventBus\" не установлен в инспекторе!");
    }

    private void Start()
    { 
        _canSpawn = false;
    }

    private void OnEnable()
    {
        if (_eventBus != null)
        {
            _eventBus.GameStarted += OnGameStarted;
            _eventBus.GameRestarted += OnGameRestarted;
            _eventBus.PlayerDied += OnPlayerDied;
        }
    }

    private void OnDisable()
    {
        if (_eventBus != null)
        {
            _eventBus.GameStarted -= OnGameStarted;
            _eventBus.GameRestarted -= OnGameRestarted;
            _eventBus.PlayerDied -= OnPlayerDied;
        }
    }

    public void ResetGenerator() =>
        _enemyPool?.ResetPool();

    private void OnGameStarted()
    {
        _canSpawn = true;
        StartCoroutine(SpawnEnemies());
    }

    private void OnGameRestarted()
    {
        _canSpawn = true;
        StartCoroutine(SpawnEnemies());
    }

    private void OnPlayerDied()
    {
        _canSpawn = false;
        StopAllCoroutines(); 
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

        _enemyPool.GetEnemy(spawnPoint);
    }
}