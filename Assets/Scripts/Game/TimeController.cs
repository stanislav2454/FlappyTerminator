using UnityEngine;

public class TimeController : MonoBehaviour
{
    private const float TimeScaleReal = 1f;
    private const float RealTimePause = 0f;

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

    private void Start()
    {
        Time.timeScale = RealTimePause;
    }

    private void OnGameStarted() =>
        SetTimeScale(TimeScaleReal);

    private void OnGameRestarted() =>
        SetTimeScale(TimeScaleReal);

    private void OnPlayerDied() =>
        SetTimeScale(RealTimePause);

    private void SetTimeScale(float scale) =>
        Time.timeScale = scale;
}