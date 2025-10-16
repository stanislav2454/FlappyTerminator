using UnityEngine;
using TMPro;
using System;

public class GameUI : MonoBehaviour
{
    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private EndGameScreen _endGameScreen;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private GameObject _hud;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private ScoreManager _scoreManager;

    public event Action OnPlayButtonClicked;
    public event Action OnRestartButtonClicked;

    private void Awake()
    {
        if (_startScreen == null)
            Debug.LogError("Компонент \"StartScreen\" не установлен в инспекторе!");

        if (_endGameScreen == null)
            Debug.LogError("Компонент \"EndGameScreen\" не установлен в инспекторе!");

        if (_scoreText == null)
            Debug.LogError("Компонент \"TMP_Text\" не установлен в инспекторе!");

        if (_hud == null)
            Debug.LogError("Компонент \"HUD (Heads-Up Display(интерфейс отображения информации на экране))\" не установлен в инспекторе!");

        if (_gameManager == null)
            Debug.LogError("Компонент \"GameManager\" не установлен в инспекторе!");

        if (_scoreManager == null)
            Debug.LogError("Компонент \"ScoreManager\" не установлен в инспекторе!");
    }

    private void Start()
    {
        InitializeUI();
    }

    private void OnEnable()
    {
        if (_gameManager != null)
        {
            _gameManager.PlayerDied += OnPlayerDied;
            _gameManager.GameStarted += OnGameStarted;
            _gameManager.GameRestarted += OnGameRestarted;
        }

        if (_scoreManager != null)
            _scoreManager.ScoreChanged += OnScoreChanged;

        if (_startScreen != null)
            _startScreen.PlayButtonClicked += OnPlayButtonClick;

        if (_endGameScreen != null)
            _endGameScreen.RestartButtonClicked += OnRestartButtonClick;
    }

    private void OnDisable()
    {
        if (_gameManager != null)
        {
            _gameManager.PlayerDied -= OnPlayerDied;
            _gameManager.GameStarted -= OnGameStarted;
            _gameManager.GameRestarted -= OnGameRestarted;
        }

        if (_scoreManager != null)
            _scoreManager.ScoreChanged -= OnScoreChanged;

        if (_startScreen != null)
            _startScreen.PlayButtonClicked -= OnPlayButtonClick;

        if (_endGameScreen != null)
            _endGameScreen.RestartButtonClicked -= OnRestartButtonClick;
    }

    private void InitializeUI()
    {
        _startScreen.Open();
        _hud.SetActive(false);
        _endGameScreen.Close();
    }

    private void OnScoreChanged(int score)
    {
        if (_scoreText != null)
            _scoreText.text = $"Score: {score}";
    }

    private void OnPlayerDied()
    {
        _endGameScreen.Open();
        _hud.SetActive(false);
    }

    private void OnGameStarted()
    {
        _startScreen.Close();
        _hud.SetActive(true);
    }

    private void OnGameRestarted()
    {
        _endGameScreen.Close();
        _hud.SetActive(true);
    }

    private void OnPlayButtonClick() =>
        OnPlayButtonClicked?.Invoke();
    private void OnRestartButtonClick() =>
        OnRestartButtonClicked?.Invoke();
}