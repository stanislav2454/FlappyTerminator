using UnityEngine;

[RequireComponent(typeof(Health), typeof(EnemyWeapon))]
public class Enemy : MonoBehaviour, IInteractable, IDamageable
{
    private const float DefaultTimeValue = 0f;
    private const float Delay = 2f;

    [SerializeField] private int _scoreValue = 1;
    [SerializeField] private float _movementAmplitude = 1f;
    [SerializeField] private float _movementFrequency = 1f;
    [SerializeField] private EnemyWeapon _weapon;

    private GameManager _gameManager;
    private Vector3 _startPosition;
    private Health _health;
    private float _time;
    private bool _isActive;
    private Coroutine _shootingCoroutine;
    private WaitForSeconds _shootDelayWait;

    public event System.Action<Enemy> EnemyDisabled;
    public event System.Action<Enemy> EnemyDied;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _weapon = GetComponent<EnemyWeapon>();

        _shootDelayWait = new WaitForSeconds(Delay);
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

        if (_weapon != null)
            _shootingCoroutine = StartCoroutine(ShootingRoutine());
    }

    private void OnDisable()
    {
        _isActive = false;

        if (_health != null)
            _health.Died -= OnDied;

        if (_shootingCoroutine != null)
        {
            StopCoroutine(_shootingCoroutine);
            _shootingCoroutine = null;
        }

        _weapon?.StopShooting();
    }

    private void Update()
    {
        if (_isActive == false)
            return;

        _time += Time.deltaTime;

        var position = transform.position;
        position.y = _startPosition.y + Mathf.Sin(_time * _movementFrequency) * _movementAmplitude;
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.TryGetComponent<Obstacle>(out _))
            EnemyDisabled?.Invoke(this);
    }

    public void SetGameManager(GameManager gameManager) =>
        _gameManager = gameManager;

    public void TakeDamage(int damage) =>
        _health?.TakeDamage(damage);

    private System.Collections.IEnumerator ShootingRoutine()
    {
        while (_isActive)
        {
            yield return _shootDelayWait;

            if (_isActive)
                _weapon?.Shoot(transform.position);
        }
    }

    private void OnDied()
    {
        _gameManager?.EnemyDestroyed(_scoreValue);
        EnemyDied?.Invoke(this);
    }
}