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
    private void Start()
    {
        // 🔥 ЭКСТРЕННОЕ ИСПРАВЛЕНИЕ - найти пул если не установлен
        if (_bulletPool == null)
        {
            _bulletPool = FindObjectOfType<BulletPool>();
            if (_bulletPool != null)
                Debug.Log($"✅ Auto-connected {gameObject.name} to bullet pool");
            else
                Debug.LogError($"❌ No bullet pool found in scene for {gameObject.name}!");
        }
    }

    public abstract void Shoot(Vector3 position);

    protected virtual void ProcessShoot(Vector2 direction, Vector3 position)
    {
        if (_bulletPool != null)
        {
            _bulletPool.GetBullet(position, direction, _owner);

            // Мониторинг
            var monitor = FindObjectOfType<PerformanceMonitor>();
            monitor?.BulletCreated(true);
        }
        else
        {
            var bullet = Instantiate(_bulletPrefab, position, Quaternion.identity);
            bullet.Initialize(direction, _owner);
            // ✅ ДОБАВИТЬ ПРЕДУПРЕЖДЕНИЕ В ЛОГ
            Debug.LogWarning($"Weapon on {gameObject.name} is creating new bullets instead of using pool!");

            // Мониторинг
            var monitor = FindObjectOfType<PerformanceMonitor>();
            monitor?.BulletCreated(false);

            Debug.LogError($"🚨 INSTANTIATE: {gameObject.name} created new bullet!");
        }
    }

    protected virtual IEnumerator CooldownRoutine()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_cooldown);
        _canShoot = true;
        _cooldownCoroutine = null;
    }

    public virtual void StopShooting()
    {
        if (_cooldownCoroutine != null)
        {
            StopCoroutine(_cooldownCoroutine);
            _cooldownCoroutine = null;
        }
        _canShoot = true;
    }

    // ✅ НОВЫЙ МЕТОД - для установки пула извне
    public virtual void SetBulletPool(BulletPool bulletPool)
    {
        _bulletPool = bulletPool;
    }
}