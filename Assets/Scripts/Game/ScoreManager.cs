using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private const int DefaultScoreValue = 0;

    [SerializeField] private GameManager _gameManager;

    // Событие для обновления UI
    public event Action<int> ScoreChanged;

    private int _currentScore;
    public int CurrentScore => _currentScore;

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
        }
    }

    private void OnDisable()
    {
        if (_gameManager != null)
        {
            _gameManager.GameStarted -= OnGameStarted;
            _gameManager.GameRestarted -= OnGameRestarted;
        }
    }

    public void AddScore(int points)
    {
        if (points <= 0) 
            return;

        _currentScore += points;
        ScoreChanged?.Invoke(_currentScore);
    }

    public void ResetScore()
    {
        _currentScore = DefaultScoreValue;
        ScoreChanged?.Invoke(_currentScore);
    }

    private void OnGameStarted()
    {// DRY
        ResetScore();
    }

    private void OnGameRestarted()
    {// DRY
        ResetScore();
    }
}