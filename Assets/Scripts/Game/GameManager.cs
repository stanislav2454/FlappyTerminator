using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("System Dependencies")]
    [SerializeField] private PlayerController _player;
    [SerializeField] private EnemySpawner _enemyGenerator;
    [SerializeField] private TimeController _timeController;
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private GameRestarter _gameRestarter;
    [SerializeField] private ScoreManager _scoreManager;

    [SerializeField] private GameState _currentState = GameState.Menu;

    public event Action GameStarted;
    public event Action GameRestarted;
    public event Action PlayerDied;

    private void Start()
    {
        InitializeSystems();
    }

    private void OnDisable()
    {
        UnsubscribeFromSystems();
    }

    public void EnemyDestroyed(int points) =>
        _scoreManager?.AddScore(points);

    public void StartGame()
    {
        if (_currentState == GameState.Playing)
            return;

        InitializeGame(isRestart: false);
    }

    public void RestartGame()
    {
        InitializeGame(isRestart: true);
        _gameRestarter?.RestartGame();
    }

    private void InitializeSystems()
    {
        if (_player != null)
            _player.OnDied += HandlePlayerDied;

        if (_gameUI != null)
        {
            _gameUI.OnPlayButtonClicked += StartGame;
            _gameUI.OnRestartButtonClicked += RestartGame;
        }
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
}
public enum GameState
{
    Menu,
    Playing,
    GameOver
}