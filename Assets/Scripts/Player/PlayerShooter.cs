using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletShootPoint;
    [SerializeField] private EventBus _eventBus;
    [SerializeField] private BulletPool _bulletPool;
    [SerializeField] private float _cooldown = 0.5f;

    private bool _canShoot = true;

    private void Awake()
    {
        if (_eventBus == null)
            Debug.LogError("Компонент \"EventBus\" не установлен в инспекторе!");

        if (_bulletPrefab == null)
            Debug.LogError("Компонент \"Bullet Prefab\" не установлен в инспекторе!");

        if (_bulletShootPoint == null)
            Debug.LogError("Компонент \"Transform\" для \"bulletShootPoint\" не установлен в инспекторе!");

        if (_bulletPool == null)
            Debug.LogError("Компонент \"BulletPool\" для не установлен в инспекторе!");
    }

    private void OnEnable()
    {
        if (_eventBus != null)
            _eventBus.GameRestarted += OnGameRestarted;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void Shoot()
    {
        if (_canShoot == false || _bulletPrefab == null)
            return;

        Vector2 shootDirection = transform.right;
        Vector3 spawnPosition = _bulletShootPoint.position;

        if (_bulletPool != null)
        {
            var bullet = _bulletPool.GetBullet(spawnPosition, shootDirection, BulletOwner.Player);
        }
        else
        {
            var bullet = Instantiate(_bulletPrefab, spawnPosition, Quaternion.identity);
            bullet.Initialize(shootDirection, BulletOwner.Player);
        }

        StartCoroutine(CooldownRoutine());
    }

    private void OnGameRestarted()
    {
        _canShoot = true;
        StopAllCoroutines();
    }

    private System.Collections.IEnumerator CooldownRoutine()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_cooldown);
        _canShoot = true;
    }
}