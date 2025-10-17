using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _spawnDelay = 3f;
    [SerializeField] private float _minSpawnHeight = -2f;
    [SerializeField] private float _maxSpawnHeight = 3f;
    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private GameManager _gameManager;

    private bool _canSpawn = false;
    private bool _isSpawning = false;
    private WaitForSeconds _spawnWait;
    private Coroutine _spawnEnemiesRoutine;

    private void Awake()
    {
        _spawnWait = new WaitForSeconds(_spawnDelay);

        if (_enemyPool == null)
            Debug.LogError("Компонент \"EnemyPool\" не установлен в инспекторе!");

        if (_gameManager == null)
            Debug.LogError("Компонент \"GameManager\" не установлен в инспекторе!");
    }

    private void OnEnable()
    {
        if (_gameManager != null)
        {
            _gameManager.GameStarted += OnGameStarted;
            _gameManager.GameRestarted += OnGameStarted;
            _gameManager.PlayerDied += OnPlayerDied;
        }
    }

    private void OnDisable()
    {
        if (_gameManager != null)
        {
            _gameManager.GameStarted -= OnGameStarted;
            _gameManager.GameRestarted -= OnGameStarted;
            _gameManager.PlayerDied -= OnPlayerDied;
        }
    }

    public void ResetGenerator() =>
        _enemyPool?.ResetPool();

    public void StartSpawning()
    {
        if (_isSpawning)
            return;

        _isSpawning = true;
        _spawnEnemiesRoutine = StartCoroutine(SpawnEnemies());
    }

    public void StopSpawning()
    {
        _canSpawn = false;
        _isSpawning = false;

        if (_spawnEnemiesRoutine != null)
        {
            StopCoroutine(_spawnEnemiesRoutine);
            _spawnEnemiesRoutine = null;
        }
    }

    private void OnGameStarted()
    {
        _canSpawn = true;
        StartSpawning();
    }

    private void OnPlayerDied() =>
        StopSpawning();

    private IEnumerator SpawnEnemies()
    {
        while (_canSpawn)
        {
            if (_enemyPool != null && _enemyPool.GetPooledObjectsCount() > 0)
                SpawnEnemy();

            yield return _spawnWait;
        }
    }

    private void SpawnEnemy()
    {
        float spawnPositionY = Random.Range(_minSpawnHeight, _maxSpawnHeight);
        Vector3 spawnPoint = new Vector3(transform.position.x, spawnPositionY, transform.position.z);

        _enemyPool.GetEnemy(spawnPoint);
    }
}