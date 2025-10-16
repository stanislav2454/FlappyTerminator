using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour// переимновать на spawner
{
    [SerializeField] private float _spawnDelay = 3f;
    [SerializeField] private float _minSpawnHeight = -2f;
    [SerializeField] private float _maxSpawnHeight = 3f;
    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private GameManager _gameManager;

    private bool _canSpawn = false;
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
            // _gameManager.GameRestarted += OnGameRestarted;
            _gameManager.PlayerDied += OnPlayerDied;
        }
    }

    private void OnDisable()
    {
        if (_gameManager != null)
        {
            _gameManager.GameStarted -= OnGameStarted;
            _gameManager.GameRestarted -= OnGameStarted;
            // _gameManager.GameRestarted -= OnGameRestarted;
            _gameManager.PlayerDied -= OnPlayerDied;
        }
    }

    public void ResetGenerator() =>
        _enemyPool?.ResetPool();

    // Добавить защиту от множественного запуска
    private bool _isSpawning = false;
    //todo
    public void StartSpawning()
    {
        if (_isSpawning)
            return; // ← ЗАЩИТА

        _isSpawning = true;
        _spawnEnemiesRoutine = StartCoroutine(SpawnEnemies());
    }

    public void StopSpawning()
    {
        _canSpawn = false;
        _isSpawning = false; // ← СБРОС ФЛАГА
        if (_spawnEnemiesRoutine != null)
        {
            StopCoroutine(_spawnEnemiesRoutine);
            _spawnEnemiesRoutine = null;
        }
    }

    private void OnGameStarted()
    {//  зачем два одинаковых метода ?
        _canSpawn = true;
        StartSpawning();
    }

    //private void OnGameRestarted()
    //{//  зачем два одинаковых метода ?
    //    _canSpawn = true;
    //    StartSpawning();
    //}

    private void OnPlayerDied()
    {
        StopSpawning();
    }

    private IEnumerator SpawnEnemies()
    {
        while (_canSpawn)
        {
            // ✅ ПРОВЕРЯЕМ ЕСТЬ ЛИ СВОБОДНЫЕ ВРАГИ В ПУЛЕ
            if (_enemyPool != null && _enemyPool.GetPooledObjectsCount() > 0)
                SpawnEnemy();
            else
                Debug.LogWarning("EnemyPool is empty! Waiting for enemies to return...");

            yield return _spawnWait;
        }
    }

    private void SpawnEnemy()
    {
        Debug.Log($"SpawnEnemy called from: {new System.Diagnostics.StackTrace()}");
        float spawnPositionY = Random.Range(_minSpawnHeight, _maxSpawnHeight);
        Vector3 spawnPoint = new Vector3(transform.position.x, spawnPositionY, transform.position.z);

        _enemyPool.GetEnemy(spawnPoint);
    }
}