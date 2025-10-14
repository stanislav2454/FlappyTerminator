using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Bullet : MonoBehaviour, IInteractable
{
    private const int Score = 1;

    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _lifeTime = 3f;
    [SerializeField] private int _damage = 1;

    private Vector2 _direction;
    private BulletOwner _owner;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null)
            return;

        if (_owner == BulletOwner.Player && other.CompareTag("Player"))
            return;

        if (_owner == BulletOwner.Enemy && other.CompareTag("Enemy"))
            return;

        if (_owner == BulletOwner.Player && other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(_damage);

            Destroy(gameObject);
        }
        else if (_owner == BulletOwner.Enemy && other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out IDamageable damageable))
                damageable.TakeDamage(_damage);

            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground") || other.CompareTag("Ceiling") || other.CompareTag("Boundary"))
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(Vector2 direction, BulletOwner owner)
    {
        _direction = direction.normalized;
        _owner = owner;

        GetComponent<SpriteRenderer>().color = owner == BulletOwner.Player ? Color.green : Color.red;

        _rigidbody.velocity = _direction * _speed;
        Destroy(gameObject, _lifeTime);
    }
}