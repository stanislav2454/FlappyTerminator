using UnityEngine;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour, IInteractable, IDamageable
{
    [SerializeField] private int _scoreValue = 1;
    [SerializeField] private float _shootDelay = 2f;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _movementAmplitude = 1f;
    [SerializeField] private float _movementFrequency = 1f;

    private EventBus _eventBus;
    private Vector3 _startPosition;
    private Health _health;
    private float _time;
    private bool _isActive;

    public event System.Action<Enemy> EnemyDisabled;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _isActive = true;
        _startPosition = transform.position;
        _time = 0f;

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

        var bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        bullet.Initialize(Vector2.left, BulletOwner.Enemy);
    }

    private void OnDied()
    {
        _eventBus?.PublishEnemyDestroyed(_scoreValue);
        EnemyDisabled?.Invoke(this);
        gameObject.SetActive(false);
    }
}