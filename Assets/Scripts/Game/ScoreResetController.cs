using UnityEngine;

public class ScoreResetController : MonoBehaviour
{
    [SerializeField] private EventBus _eventBus;

    private void Awake()
    {
        if (_eventBus == null)
            Debug.LogError("Компонент \"EventBus\" не установлен в инспекторе!");
    }

    private void OnEnable()
    {
        if (_eventBus != null)
        {
            _eventBus.GameStarted += OnGameStarted;
            _eventBus.GameRestarted += OnGameRestarted;
        }
    }

    private void OnDisable()
    {
        if (_eventBus != null)
        {
            _eventBus.GameStarted -= OnGameStarted;
            _eventBus.GameRestarted -= OnGameRestarted;
        }
    }

    private void OnGameStarted() =>
        ResetScore();

    private void OnGameRestarted() =>
        ResetScore();

    private void ResetScore() =>
        _eventBus?.PublishScoreChanged(0);
}