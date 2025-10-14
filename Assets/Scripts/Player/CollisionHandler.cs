using UnityEngine;
using System;

public class CollisionHandler : MonoBehaviour
{
    public event Action ObstacleHit;

    [SerializeField] private string[] _obstacleTags = { "Ground", "Ceiling", "Boundary" };

    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (var tag in _obstacleTags)
        {
            if (other.CompareTag(tag))
            {
                ObstacleHit?.Invoke();
                return;
            }
        }
    }
}