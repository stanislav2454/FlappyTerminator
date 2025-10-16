using UnityEngine;
using System;

public class CollisionHandler : MonoBehaviour
{
    public event Action<ObstacleType> ObstacleHit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null)
            return;

        if (other.TryGetComponent<Obstacle>(out var obstacle))
            ObstacleHit?.Invoke(obstacle.Type);
        //// ИСПРАВЛЕНИЕ: Используем Obstacle вместо IObstacle
        //if (other.TryGetComponent<Obstacle>(out _))
        //    ObstacleHit?.Invoke();
        ////if (other.TryGetComponent<IObstacle>(out _))
        ////    ObstacleHit?.Invoke();
        //////if (other.TryGetComponent<Boundary>(out _) || other.TryGetComponent<Ground>(out _) || other.TryGetComponent<Ceiling>(out _))
        //////    ObstacleHit?.Invoke();
    }
}