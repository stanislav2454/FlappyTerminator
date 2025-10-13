using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Bullet : MonoBehaviour, IInteractable, IBullet
{
    private const int Score = 1;

    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _lifeTime = 3f;

    private Vector2 _direction;
    private BulletOwner _owner;
    private Rigidbody2D _rigidbody;

    public static event System.Action<int> EnemyHit; //todo static!

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction, BulletOwner owner)
    {
        _direction = direction.normalized;
        _owner = owner;

        GetComponent<SpriteRenderer>().color = owner == BulletOwner.Player ? Color.green : Color.red;

        _rigidbody.velocity = _direction * _speed;
        Destroy(gameObject, _lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null)
            return;

        if (_owner == BulletOwner.Player && other.CompareTag("Player"))
            return;

        if (_owner == BulletOwner.Player && other.CompareTag("Enemy"))
        {
            EnemyHit?.Invoke(Score); // +1 очко за врага
            Destroy(other.gameObject); // Уничтожить врага
            Destroy(gameObject); // Уничтожить пулю
        }
        else if (_owner == BulletOwner.Enemy && other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("ObjectRemover"))
        {
            Destroy(gameObject); // Уничтожить пулю при столкновении с землей или границей
        }
    }
}