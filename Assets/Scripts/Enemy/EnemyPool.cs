using UnityEngine;
using System.Collections.Generic;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private int _poolSize = 10;
    [SerializeField] private Transform _container;
    [SerializeField] private EventBus _eventBus;

    private Queue<Enemy> _pool = new Queue<Enemy>();
    private List<Enemy> _activeEnemies = new List<Enemy>();

    private void Awake()
    {
        InitializePool();
    }

    public Enemy GetEnemy(Vector3 position)
    {
        Enemy enemy;

        if (_pool.Count > 0)
            enemy = _pool.Dequeue();
        else
            enemy = CreateNewEnemy();

        enemy.transform.position = position;
        enemy.gameObject.SetActive(true);
        _activeEnemies.Add(enemy);

        return enemy;
    }

    public void ReturnEnemy(Enemy enemy)
    {
        if (enemy == null)
            return;

        enemy.gameObject.SetActive(false);
        _activeEnemies.Remove(enemy);
        _pool.Enqueue(enemy);
    }

    public void ResetPool()
    {
        foreach (var enemy in _activeEnemies.ToArray())
            ReturnEnemy(enemy);
    }

    private void InitializePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            var enemy = CreateNewEnemy();
            _pool.Enqueue(enemy);
        }
    }

    private Enemy CreateNewEnemy()
    {
        var enemy = Instantiate(_enemyPrefab, _container);
        enemy.SetEventBus(_eventBus);
        enemy.EnemyDisabled += ReturnEnemy;
        enemy.gameObject.SetActive(false);
        return enemy;
    }
}