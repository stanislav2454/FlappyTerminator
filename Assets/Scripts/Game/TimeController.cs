using UnityEngine;

public class TimeController : MonoBehaviour
{
    private const float TimeScaleReal = 1f;
    private const float RealTimePause = 0f;

    [SerializeField] private GameManager _gameManager;

    private void Awake()
    {
        if (_gameManager == null)
            Debug.LogError("Компонент \"GameManager\" не установлен в инспекторе!");
    }

    private void OnEnable()
    {
        if (_gameManager != null)
        {
            _gameManager.GameStarted += OnGameStarted;
            _gameManager.GameRestarted += OnGameRestarted;
            _gameManager.PlayerDied += OnPlayerDied;
        }
    }

    private void OnDisable()
    {
        if (_gameManager != null)
        {
            _gameManager.GameStarted -= OnGameStarted;
            _gameManager.GameRestarted -= OnGameRestarted;
            _gameManager.PlayerDied -= OnPlayerDied;
        }
    }

    private void Start()
    {
        Time.timeScale = RealTimePause;
    }

    public void SetNormalTime() =>
        Time.timeScale = TimeScaleReal;

    public void SetPausedTime() =>
        Time.timeScale = RealTimePause;

    private void OnGameStarted() =>
        SetNormalTime();

    private void OnGameRestarted() =>
        SetNormalTime();

    private void OnPlayerDied() =>
        SetPausedTime();
}