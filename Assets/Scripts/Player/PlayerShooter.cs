using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletShootPoint;
    [SerializeField] private float _cooldown = 0.5f;
    [SerializeField] private EventBus _eventBus;

    private bool _canShoot = true;

    private void Awake()
    {
        if (_eventBus == null)
            Debug.LogError("Компонент \"EventBus\" не установлен в инспекторе!");

        if (_bulletPrefab == null)
            Debug.LogError("Компонент \"Bullet Prefab\" не установлен в инспекторе!");

        if (_bulletShootPoint == null)
            Debug.LogError("Компонент \"Transform\" для \"bulletShootPoint\" не установлен в инспекторе!");
    }

    private void OnEnable()
    {
        if (_eventBus != null)
        {
            _eventBus.GameRestarted += OnGameRestarted;
            Debug.Log("[PlayerShooter] Subscribed to GameRestarted event");
        }
    }

    private void OnDisable()
    {
        //if (_eventBus != null)
        //{
        //    _eventBus.GameRestarted -= OnGameRestarted;
        //    Debug.Log("[PlayerShooter] Unsubscribed from GameRestarted event");
        //}
        StopAllCoroutines();
        Debug.Log("[PlayerShooter] Stopped all coroutines");
    }

    public void Shoot()
    {
        //if (_canShoot == false || _bulletPrefab == null)
        //    return;
        if (_canShoot == false)
        {
            Debug.Log($"[PlayerShooter] Shoot blocked - cooldown active");
            return;
        }

        if (_bulletPrefab == null)
        {
            Debug.LogError("[PlayerShooter] Bullet prefab is null!");
            return;
        }

        Vector2 shootDirection = transform.right;
        Vector3 spawnPosition = _bulletShootPoint.position;
        //Vector3 spawnPosition = new Vector3(_bulletShootPoint.position.x, _bulletShootPoint.position.y, _bulletShootPoint.position.z);

        var bullet = Instantiate(_bulletPrefab, spawnPosition, Quaternion.identity);
        bullet.Initialize(shootDirection, BulletOwner.Player);

        StartCoroutine(CooldownRoutine());
    }

    private void OnGameRestarted()
    {
        Debug.Log("[PlayerShooter] Game restarted - resetting shoot state");
        _canShoot = true; // Сбросить состояние кд
        StopAllCoroutines(); // Остановить все корутины
    }

    private System.Collections.IEnumerator CooldownRoutine()
    {
        _canShoot = false;
        Debug.Log("[PlayerShooter] Cooldown started");
        yield return new WaitForSeconds(_cooldown);
        _canShoot = true;
        Debug.Log("[PlayerShooter] Cooldown finished - can shoot again");
    }
}