using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameState _currentState = GameState.Menu;
    [SerializeField] private EventBus _eventBus;

    public GameState CurrentState => _currentState;

    private void Awake()
    {
        if (_eventBus == null)
            Debug.LogError("Компонент \"EventBus\" не установлен в инспекторе!");
    }

    private void OnEnable()
    {
        if (_eventBus != null)
        {
            _eventBus.PlayerDied += OnPlayerDied;
            _eventBus.GameStarted += OnGameStarted;
            _eventBus.GameRestarted += OnGameRestarted;
        }
    }

    private void OnDisable()
    {
        if (_eventBus != null)
        {
            _eventBus.PlayerDied -= OnPlayerDied;
            _eventBus.GameStarted -= OnGameStarted;
            _eventBus.GameRestarted -= OnGameRestarted;
        }
    }

    private void OnGameStarted() =>
        _currentState = GameState.Playing;

    private void OnGameRestarted() =>
        _currentState = GameState.Playing;

    private void OnPlayerDied() =>
        _currentState = GameState.GameOver;
}