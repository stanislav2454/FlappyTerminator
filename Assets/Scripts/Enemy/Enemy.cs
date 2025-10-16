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

    private void Awake()
    {
        _health = GetComponent<Health>();

        if (_weapon == null)
            Debug.LogError("Компонент \"EnemyWeapon\" не установлен в инспекторе!");

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

        //StopAllCoroutines();//!!!!!!!!!!!!!!
        _weapon?.StopShooting();
    }

    private void Update()
    {
        if (_isActive == false)
            return;

        _time += Time.deltaTime;
        // ✅ ОПТИМИЗИРОВАННОЕ ДВИЖЕНИЕ - меньше аллокаций
        var position = transform.position;
        position.y = _startPosition.y + Mathf.Sin(_time * _movementFrequency) * _movementAmplitude;
        transform.position = position;

        //float newY = _startPosition.y + Mathf.Sin(_time * _movementFrequency) * _movementAmplitude;

        //var position = transform.position;
        //position.y = newY;
        //transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.TryGetComponent<Obstacle>(out _))
        {
            EnemyDisabled?.Invoke(this);
            gameObject.SetActive(false);
        }
    }

    public void SetGameManager(GameManager gameManager) =>
        _gameManager = gameManager;

    public void SetBulletPool(BulletPool bulletPool)
    {
        // ✅ ПЕРЕДАЕМ ПУЛ ОРУЖИЮ, а не просто пустой метод
        if (_weapon != null)
        {
            _weapon.SetBulletPool(bulletPool);
            Debug.Log($"✅ Set bullet pool for enemy weapon: {_weapon != null}");
        }
        else
        {
            Debug.LogError($"❌ Enemy weapon is null!");
        }
    }

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
        EnemyDisabled?.Invoke(this);
        gameObject.SetActive(false);
    }
}