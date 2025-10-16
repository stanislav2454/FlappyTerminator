using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private ObstacleType _type = ObstacleType.Solid;

    public ObstacleType Type => _type;
}

public enum ObstacleType
{
    Solid,      // Полностью блокирует
    Boundary,   // Границы экрана
    Ground      // Земля (может иметь особое поведение)
}