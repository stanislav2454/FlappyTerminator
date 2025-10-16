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
    }
}