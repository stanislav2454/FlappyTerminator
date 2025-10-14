using UnityEngine;
/// <summary>
/// Класс обрабатывает столкновение границы экрана с врагами.
/// </summary>
public class BoundaryController : MonoBehaviour
{
    [SerializeField] private EnemyPool _enemyPool;

    private void Awake()
    {
        if (_enemyPool == null)
            Debug.LogError("Компонент \"EnemyPool\" не установлен в инспекторе!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Enemy enemy))
            _enemyPool?.ReturnEnemy(enemy);
    }
}