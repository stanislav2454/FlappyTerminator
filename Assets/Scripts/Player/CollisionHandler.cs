using UnityEngine;
using System;

public class CollisionHandler : MonoBehaviour
{
    public event Action ObstacleHit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null)
            return;

        if (other.TryGetComponent<Boundary>(out _) || other.TryGetComponent<Ground>(out _) || other.TryGetComponent<Ceiling>(out _))
            ObstacleHit?.Invoke();
    }
}