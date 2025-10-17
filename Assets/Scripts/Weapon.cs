using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected Bullet _bulletPrefab;
    [SerializeField] protected float _cooldown = 0.5f;
    [SerializeField] protected Transform _shootPoint;
    [SerializeField] protected LayerMask _friendlyLayers;

    protected bool _canShoot = true;
    protected Coroutine _cooldownCoroutine;
    protected BulletOwner _owner;

    public event System.Action<Vector3, Vector2, BulletOwner, LayerMask> ShootRequested;

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

    protected virtual void ProcessShoot(Vector2 direction, Vector3 position)
    {
        ShootRequested?.Invoke(position, direction, _owner, _friendlyLayers);
    }

    protected virtual IEnumerator CooldownRoutine()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_cooldown);
        _canShoot = true;
        _cooldownCoroutine = null;
    }
}