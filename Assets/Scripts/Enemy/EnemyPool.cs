using UnityEngine;

public class EnemyPool : GenericPool<Enemy>
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private BulletPool _bulletPool;

    protected override Enemy CreateNewObject()
    {
        var enemy = base.CreateNewObject();

        if (enemy != null)
        {
            enemy.SetGameManager(_gameManager);
            enemy.EnemyDisabled += ReturnObject;

            if (_bulletPool != null)
                enemy.SetBulletPool(_bulletPool);
        }

        return enemy;
    }

    public Enemy GetEnemy(Vector3 position)
    {
        var enemy = GetObject(position);

        return enemy;
    }
}