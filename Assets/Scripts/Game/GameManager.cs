using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("System Dependencies")]
    [SerializeField] private PlayerController _player;
    [SerializeField] private EnemyGenerator _enemyGenerator;
    [SerializeField] private TimeController _timeController;
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private GameRestarter _gameRestarter;
    [SerializeField] private ScoreManager _scoreManager;

    [SerializeField] private GameState _currentState = GameState.Menu;

    public event Action GameStarted;
    public event Action GameRestarted;
    public event Action PlayerDied;

    public GameState CurrentState => _currentState;


    private void Start()
    {
        InitializeSystems();
        // Принудительно настроить все оружие
        EmergencyWeaponFix(); // 🔥 ДОБАВЬТЕ ЭТУ СТРОКУ
        ConfigureAllWeapons();
    }
    // мониторинг начало// после отладки удалить!
    [SerializeField] private BulletPool _bulletPool;
    [SerializeField] private EnemyPool _enemyPool;
    private void EmergencyWeaponFix()
    {
        StartCoroutine(DelayedWeaponFix());
    }
    private IEnumerator DelayedWeaponFix()
    {
        yield return new WaitForSeconds(1f); // Дать игре запуститься

        var bulletPool = FindObjectOfType<BulletPool>();
        if (bulletPool != null)
        {
            var playerWeapon = FindObjectOfType<PlayerWeapon>();
            if (playerWeapon != null)            
                playerWeapon.SetBulletPool(bulletPool);            
        }
    }
    private void ConfigureAllWeapons()
    {
        if (_enemyPool != null)
        {
            var enemies = FindObjectsOfType<Enemy>();
            foreach (var enemy in enemies)
            {
                var weapon = enemy.GetComponent<EnemyWeapon>();
                if (weapon != null)
                {
                    weapon.SetBulletPool(_bulletPool);
                    Debug.Log($"✅ Configured enemy weapon with bullet pool");
                }
            }
        }

        // Настроить игрока
        if (_player != null)
        {
            var weapon = _player.GetComponent<PlayerWeapon>();
            if (weapon != null)
            {
                weapon.SetBulletPool(_bulletPool);
                Debug.Log($"✅ Configured player weapon with bullet pool");
            }
        }
    }
    // мониторинг конец
    private void OnEnable()
    {
        PlayerDied += OnPlayerDiedInternal;
    }

    private void OnDisable()
    {
        PlayerDied -= OnPlayerDiedInternal;
        UnsubscribeFromSystems();
    }

    private void UnsubscribeFromSystems()
    {
        if (_player != null)
            _player.OnDied -= HandlePlayerDied;

        if (_gameUI != null)
        {
            _gameUI.OnPlayButtonClicked -= StartGame;
            _gameUI.OnRestartButtonClicked -= RestartGame;
        }
    }

    private void InitializeSystems()
    {
        if (_player != null)
        {
            _player.OnDied += HandlePlayerDied;
        }

        if (_gameUI != null)
        {
            _gameUI.OnPlayButtonClicked += StartGame;
            _gameUI.OnRestartButtonClicked += RestartGame;
        }
    }

    public void EnemyDestroyed(int points)
    {
        _scoreManager?.AddScore(points);
    }

    public void StartGame()
    {
        if (_currentState == GameState.Playing) 
            return; // Защита от повторного запуска

        InitializeGame(isRestart: false);
    }

    public void RestartGame()
    {
        InitializeGame(isRestart: true);
        _gameRestarter?.RestartGame();
    }

    private void InitializeGame(bool isRestart)
    {
        _currentState = GameState.Playing;
        _scoreManager?.ResetScore();

        if (isRestart)
            GameRestarted?.Invoke();
        else
            GameStarted?.Invoke();

        StartGameSystems();
    }

    private void StartGameSystems()
    {
        _enemyGenerator?.StartSpawning();
        _timeController?.SetNormalTime();
    }

    private void HandlePlayerDied()
    {
        _currentState = GameState.GameOver;
        PlayerDied?.Invoke();

        _enemyGenerator?.StopSpawning();
        _timeController?.SetPausedTime();
    }
    private void OnPlayerDiedInternal()
    { // Дополнительная логика при смерти игрока (если нужна)
        Debug.Log("Game Over! Final Score: " + (_scoreManager?.CurrentScore ?? 0));
    }
}