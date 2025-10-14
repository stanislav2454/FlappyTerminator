using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const float TimeScaleReal = 1f;
    private const float RealTimePause = 0f;

    [SerializeField] private GameState _currentState = GameState.Menu;
    [SerializeField] private EventBus _eventBus;

    public GameState CurrentState => _currentState;

    private void OnEnable()
    {
        if (_eventBus != null)
        {
            _eventBus.PlayerDied += OnPlayerDied;
            _eventBus.GameStarted += OnGameStarted;
            _eventBus.GameRestarted += OnGameRestarted;
        }
    }

    private void OnDestroy()
    {
        if (_eventBus != null)
        {
            _eventBus.PlayerDied -= OnPlayerDied;
            _eventBus.GameStarted -= OnGameStarted;
            _eventBus.GameRestarted -= OnGameRestarted;
        }
    }

    private void OnGameStarted()
    {
        _currentState = GameState.Playing;
        Time.timeScale = TimeScaleReal;
    }

    private void OnGameRestarted()
    {
        _currentState = GameState.Playing;
        Time.timeScale = TimeScaleReal;
    }

    private void OnPlayerDied()
    {
        _currentState = GameState.GameOver;
        Time.timeScale = RealTimePause;
    }
}