using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private EndGameScreen _endGameScreen;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private GameObject _hud;
    [SerializeField] private EventBus _eventBus;

    private void Awake()
    {
        //Time.timeScale = 0f;

        if (_eventBus == null)
            Debug.LogError("Компонент \"EventBus\" не установлен в инспекторе!");
    }

    private void Start()
    {
        InitializeUI();
        //Time.timeScale = 0f;
    }

    private void OnEnable()
    {
        if (_eventBus != null)
        {
            _eventBus.ScoreChanged += OnScoreChanged;
            _eventBus.PlayerDied += OnPlayerDied;
            _eventBus.GameStarted += OnGameStarted;
            _eventBus.GameRestarted += OnGameRestarted;
        }

        _startScreen.PlayButtonClicked += OnPlayButtonClick;
        _endGameScreen.RestartButtonClicked += OnRestartButtonClick;
    }

    private void OnDisable()
    {
        if (_eventBus != null)
        {
            _eventBus.ScoreChanged -= OnScoreChanged;
            _eventBus.PlayerDied -= OnPlayerDied;
            _eventBus.GameStarted -= OnGameStarted;
            _eventBus.GameRestarted -= OnGameRestarted;
        }

        _startScreen.PlayButtonClicked -= OnPlayButtonClick;
        _endGameScreen.RestartButtonClicked -= OnRestartButtonClick;
    }

    private void InitializeUI()
    {
        _startScreen.Open();
        _hud.SetActive(false);
        _endGameScreen.Close();
    }

    private void OnScoreChanged(int score) =>
        _scoreText.text = $"Score: {score}";

    private void OnPlayerDied()
    {
        _endGameScreen.Open();
        _hud.SetActive(false);
        //Time.timeScale = 0f;
    }

    private void OnGameStarted()
    {
        _startScreen.Close();
        _hud.SetActive(true);
        //Time.timeScale = 1f;
        //_eventBus?.PublishScoreChanged(0);
    }

    private void OnGameRestarted()
    {
        _endGameScreen.Close();
        _hud.SetActive(true);
        //Time.timeScale = 1f;
        //_eventBus?.PublishScoreChanged(0);
    }

    private void OnPlayButtonClick() =>
        _eventBus?.PublishGameStarted();

    private void OnRestartButtonClick() =>
        _eventBus?.PublishGameRestarted();
}