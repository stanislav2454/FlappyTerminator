using System;
using UnityEngine;

[RequireComponent(typeof(PlayerWeapon))]
[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(CollisionHandler))]
public class PlayerController : MonoBehaviour
{
    private const int Damage = 1;

    [SerializeField] private PlayerWeapon _weapon;

    private Mover _mover;
    private Health _health;
    private InputHandler _inputHandler;
    private CollisionHandler _collisionHandler;

    public event Action OnDied;

    private void Awake()
    {
        _weapon = GetComponent<PlayerWeapon>();
        _mover = GetComponent<Mover>();
        _health = GetComponent<Health>();
        _inputHandler = GetComponent<InputHandler>();
        _collisionHandler = GetComponent<CollisionHandler>();
    }

    private void OnEnable()
    {
        _inputHandler.JumpPressed += OnJumpPressed;
        _inputHandler.ShootPressed += OnShootPressed;
        _health.Died += OnDiedInternal;
        _collisionHandler.ObstacleHit += OnObstacleHit;
    }

    private void OnDisable()
    {
        _inputHandler.JumpPressed -= OnJumpPressed;
        _inputHandler.ShootPressed -= OnShootPressed;
        _health.Died -= OnDiedInternal;
        _collisionHandler.ObstacleHit -= OnObstacleHit;
    }

    public void ResetPlayer()
    {
        _mover?.Reset();
        _health?.ResetHealth();
        _weapon?.StopShooting();
        gameObject.SetActive(true);
    }

    private void OnObstacleHit(ObstacleType type)
    {
        if (type == ObstacleType.Solid)
            _health?.TakeDamage(Damage);
    }

    private void OnJumpPressed() =>
        _mover?.Jump();

    private void OnShootPressed() =>
        _weapon?.Shoot(transform.position);

    private void OnDiedInternal()
    {
        OnDied?.Invoke();
        gameObject.SetActive(false);
    }
}