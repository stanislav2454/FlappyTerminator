using System;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public event Action<int> EnemyDestroyed;
    public event Action PlayerDied;
    public event Action GameStarted;
    public event Action GameRestarted;
    public event Action<int> ScoreChanged;

    public void PublishEnemyDestroyed(int points) =>
        EnemyDestroyed?.Invoke(points);

    public void PublishPlayerDied() =>
        PlayerDied?.Invoke();

    public void PublishGameStarted() =>
        GameStarted?.Invoke();

    public void PublishGameRestarted() =>
        GameRestarted?.Invoke();

    public void PublishScoreChanged(int score) =>
        ScoreChanged?.Invoke(score);
}