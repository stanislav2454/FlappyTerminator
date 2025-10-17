using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Bullet : MonoBehaviour, IInteractable
{
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _lifeTime = 3f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private LayerMask _friendlyLayers;

    private Vector2 _direction;
    private BulletOwner _owner;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Coroutine _lifeTimeCoroutine;

    public event System.Action<Bullet> ReturnedToPool;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDisable()
    {
        if (_lifeTimeCoroutine != null)
        {
            StopCoroutine(_lifeTimeCoroutine);
            _lifeTimeCoroutine = null;
        }

        _rigidbody.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null)
            return;

        if (TryHandleCollision(other))
            ReturnToPool();
    }

    public void Initialize(Vector2 direction, BulletOwner owner)
    {
        _direction = direction.normalized;
        _owner = owner;

        _spriteRenderer.color = owner == BulletOwner.Player ? Color.green : Color.red;

        _rigidbody.velocity = _direction * _speed;

        if (_lifeTimeCoroutine != null)
            StopCoroutine(_lifeTimeCoroutine);

        _lifeTimeCoroutine = StartCoroutine(LifeTimeRoutine());
    }

    public void SetFriendlyLayers(LayerMask friendlyLayers) =>
        _friendlyLayers = friendlyLayers;

    private System.Collections.IEnumerator LifeTimeRoutine()
    {
        yield return new WaitForSeconds(_lifeTime);
        ReturnToPool();
    }

    private bool TryHandleCollision(Collider2D other)
    {
        if (IsFriendlyFire(other))
            return false;

        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_damage);
            return true;
        }

        return other.TryGetComponent(out Obstacle _);
    }

    private bool IsFriendlyFire(Collider2D other) =>
         (_friendlyLayers.value & (1 << other.gameObject.layer)) != 0;

    private void ReturnToPool() =>
        ReturnedToPool?.Invoke(this);
}