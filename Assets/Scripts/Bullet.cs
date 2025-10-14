using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Bullet : MonoBehaviour, IInteractable
{
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _lifeTime = 3f;
    [SerializeField] private int _damage = 1;

    private Vector2 _direction;
    private BulletOwner _owner;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private BulletPool _bulletPool;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(ReturnToPool));
        _rigidbody.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null)
            return;

        if (_owner == BulletOwner.Player && other.TryGetComponent<PlayerController>(out _))
            return;

        if (_owner == BulletOwner.Enemy && other.TryGetComponent<Enemy>(out _))
            return;

        if (_owner == BulletOwner.Player && other.TryGetComponent<Enemy>(out _))
        {
            if (other.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(_damage);

            ReturnToPool();
        }
        else if (_owner == BulletOwner.Enemy && other.TryGetComponent<PlayerController>(out _))
        {
            if (other.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(_damage);

            ReturnToPool();
        }
        else if (other.TryGetComponent<Boundary>(out _))
        {
            ReturnToPool();
        }
    }

    public void Initialize(Vector2 direction, BulletOwner owner)
    {
        _direction = direction.normalized;
        _owner = owner;

        _spriteRenderer.color = owner == BulletOwner.Player ? Color.green : Color.red;
        
        _rigidbody.velocity = _direction * _speed;

        CancelInvoke(nameof(ReturnToPool));
        Invoke(nameof(ReturnToPool), _lifeTime);
    }

    public void SetPool(BulletPool pool) =>
        _bulletPool = pool;

    private void ReturnToPool()
    {
        if (_bulletPool != null)
            _bulletPool.ReturnBullet(this);
        else
            Destroy(gameObject);
    }
}