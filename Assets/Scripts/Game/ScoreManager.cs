using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private const int DefaultScoreValue = 0;

    [SerializeField] private GameManager _gameManager;

    private int _currentScore;

    public event Action<int> ScoreChanged;

    private void Awake()
    {
        if (_gameManager == null)
            Debug.LogError("Компонент \"GameManager\" не установлен в инспекторе!");
    }

    private void OnEnable()
    {
        if (_gameManager != null)
        {
            _gameManager.GameStarted += ResetScore;
            _gameManager.GameRestarted += ResetScore;
        }
    }

    private void OnDisable()
    {
        if (_gameManager != null)
        {
            _gameManager.GameStarted -= ResetScore;
            _gameManager.GameRestarted -= ResetScore;
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
}