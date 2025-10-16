using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected Bullet _bulletPrefab;
    [SerializeField] protected BulletPool _bulletPool;
    [SerializeField] protected float _cooldown = 0.5f;

    protected bool _canShoot = true;
    protected Coroutine _cooldownCoroutine;
    protected BulletOwner _owner;

    public abstract void Shoot(Vector3 position);

    public virtual void StopShooting()
    {
        if (_cooldownCoroutine != null)
        {
            StopCoroutine(_cooldownCoroutine);
            _cooldownCoroutine = null;
        }
        _canShoot = true;
    }

    public virtual void SetBulletPool(BulletPool bulletPool) =>
        _bulletPool = bulletPool;

    protected virtual void ProcessShoot(Vector2 direction, Vector3 position)
    {
        if (_bulletPool != null)
        {
            _bulletPool.GetBullet(position, direction, _owner);
        }
        else
        {
            var bullet = Instantiate(_bulletPrefab, position, Quaternion.identity);
            bullet.Initialize(direction, _owner);
        }
    }

    protected virtual IEnumerator CooldownRoutine()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_cooldown);
        _canShoot = true;
        _cooldownCoroutine = null;
    }
}