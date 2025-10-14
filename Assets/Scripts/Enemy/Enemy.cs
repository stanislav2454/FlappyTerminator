using UnityEngine;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour, IInteractable, IDamageable
{
    private const float DefaultTimeValue = 0f;

    [SerializeField] private int _scoreValue = 1;
    [SerializeField] private float _shootDelay = 2f;
    [SerializeField] private float _movementAmplitude = 1f;
    [SerializeField] private float _movementFrequency = 1f;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private BulletPool _bulletPool;

    private EventBus _eventBus;
    private Vector3 _startPosition;
    private Health _health;
    private float _time;
    private bool _isActive;

    public event System.Action<Enemy> EnemyDisabled;

    private void Awake()
    {
        _health = GetComponent<Health>();

        if (_bulletPrefab == null)
            Debug.LogError("Компонент \"Bullet Prefab\" не установлен в инспекторе!");

        if (_bulletPool == null)
            Debug.LogWarning("Компонент \"BulletPool\" не установлен в инспекторе!");
    }

    private void OnEnable()
    {
        _isActive = true;
        _startPosition = transform.position;
        _time = DefaultTimeValue;

        if (_health != null)
        {
            _health.Died += OnDied;
            _health.ResetHealth();
        }

        StartCoroutine(ShootingRoutine());
    }

    private void OnDisable()
    {
        _isActive = false;

        if (_health != null)
            _health.Died -= OnDied;
    }

    private void Update()
    {
        if (_isActive == false)
            return;

        _time += Time.deltaTime;
        float newY = _startPosition.y + Mathf.Sin(_time * _movementFrequency) * _movementAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    public void SetEventBus(EventBus eventBus) =>
        _eventBus = eventBus;

    public void SetBulletPool(BulletPool bulletPool) =>
        _bulletPool = bulletPool;

    public void TakeDamage(int damage) =>
        _health?.TakeDamage(damage);

    private System.Collections.IEnumerator ShootingRoutine()
    {
        while (_isActive)
        {
            yield return new WaitForSeconds(_shootDelay);
            if (_isActive)
                Shoot();
        }
    }

    private void Shoot()
    {
        if (_bulletPrefab == null)
            return;

        Vector2 shootDirection = Vector2.left;
        Vector3 spawnPosition = transform.position;

        if (_bulletPool != null)
        {
            var bullet = _bulletPool.GetBullet(spawnPosition, shootDirection, BulletOwner.Enemy);
        }
        else
        {
            var bullet = Instantiate(_bulletPrefab, spawnPosition, Quaternion.identity);
            bullet.Initialize(shootDirection, BulletOwner.Enemy);
        }
    }

    private void OnDied()
    {
        _eventBus?.PublishEnemyDestroyed(_scoreValue);
        EnemyDisabled?.Invoke(this);
        gameObject.SetActive(false);
    }
}