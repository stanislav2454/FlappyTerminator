using UnityEngine;

public class EnemyPool : GenericPool<Enemy>
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private BulletPool _bulletPool;

    protected virtual void OnDestroy()
    {
        foreach (var enemy in _activeObjects)
        {
            if (enemy != null)
            {
                enemy.EnemyDisabled -= ReturnEnemy;
                enemy.EnemyDied -= ReturnEnemy;

                var weapon = enemy.GetComponent<EnemyWeapon>();
                if (weapon != null)
                    weapon.ShootRequested -= HandleShootRequest;
            }
        }

        foreach (var enemy in _pool)
        {
            if (enemy != null)
            {
                enemy.EnemyDisabled -= ReturnEnemy;
                enemy.EnemyDied -= ReturnEnemy;
            }
        }
    }

    protected override Enemy CreateNewObject()
    {
        var enemy = base.CreateNewObject();

        if (enemy != null)
        {
            enemy.SetGameManager(_gameManager);
            enemy.EnemyDisabled += ReturnEnemy;
            enemy.EnemyDied += ReturnEnemy;
        }

        return enemy;
    }

    public override Enemy GetObject(Vector3 position)
    {
        var enemy = base.GetObject(position);

        if (enemy != null && _bulletPool != null)
        {
            var weapon = enemy.GetComponent<EnemyWeapon>();

            if (weapon != null)
                weapon.ShootRequested += HandleShootRequest;
        }

        return enemy;
    }

    public Enemy GetEnemy(Vector3 position) =>
         GetObject(position);

    public void ReturnEnemy(Enemy enemy)
    { 
        var weapon = enemy.GetComponent<EnemyWeapon>();
        if (weapon != null)
            weapon.ShootRequested -= HandleShootRequest;

        ReturnObject(enemy);
    }

    private void HandleShootRequest(Vector3 position, Vector2 direction, BulletOwner owner, LayerMask friendlyLayers)
    {
        var bullet = _bulletPool.GetBullet(position, direction, owner);
        bullet?.SetFriendlyLayers(friendlyLayers);
    }
}