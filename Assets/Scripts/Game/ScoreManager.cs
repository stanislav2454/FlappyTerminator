using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private EventBus _eventBus;

    private int _currentScore;

    public int CurrentScore => _currentScore;

    private void Awake()
    {
        if (_eventBus == null)
            Debug.LogError("Компонент \"EventBus\" не установлен в инспекторе!");
    }

    private void OnEnable()
    {
        if (_eventBus != null)
        {
            _eventBus.EnemyDestroyed += OnEnemyDestroyed;
            _eventBus.GameRestarted += OnGameRestarted;
            _eventBus.GameStarted += OnGameStarted;
        }
    }

    private void OnDisable()
    {
        if (_eventBus != null)
        {
            _eventBus.EnemyDestroyed -= OnEnemyDestroyed;
            _eventBus.GameRestarted -= OnGameRestarted;
            _eventBus.GameStarted -= OnGameStarted;
        }
    }

    private void OnEnemyDestroyed(int points)
    {
        _currentScore += points;
        _eventBus?.PublishScoreChanged(_currentScore);
    }

    private void OnGameRestarted()
    {
        _currentScore = 0;
        _eventBus?.PublishScoreChanged(_currentScore);
    }

    private void OnGameStarted()
    {
        _currentScore = 0;
        _eventBus?.PublishScoreChanged(_currentScore);
    }
}