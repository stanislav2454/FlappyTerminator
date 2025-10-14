using System;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(PlayerShooter))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(InputHandler))]
[RequireComponent(typeof(CollisionHandler))]
public class PlayerController : MonoBehaviour
{
    private const int Damage = 1;

    [SerializeField] private EventBus _eventBus;

    private PlayerShooter _shooter;
    private Mover _mover;
    private Health _health;
    private InputHandler _inputHandler;
    private CollisionHandler _collisionHandler;

    //public event Action GameOver;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
        _shooter = GetComponent<PlayerShooter>();
        _health = GetComponent<Health>();
        _inputHandler = GetComponent<InputHandler>();
        _collisionHandler = GetComponent<CollisionHandler>();

        if (_eventBus == null)
            Debug.LogError("Компонент \"EventBus\" не установлен в инспекторе!");
    }

    private void OnEnable()
    {
        _inputHandler.JumpPressed += OnJumpPressed;
        _inputHandler.ShootPressed += OnShootPressed;
        _health.Died += OnDied;
        _collisionHandler.ObstacleHit += OnObstacleHit;
    }

    private void OnDisable()
    {
        _inputHandler.JumpPressed -= OnJumpPressed;
        _inputHandler.ShootPressed -= OnShootPressed;
        _health.Died -= OnDied;
        _collisionHandler.ObstacleHit -= OnObstacleHit;
    }

    public void ResetPlayer()
    {
        Debug.Log("[PlayerController] Resetting player...");

        _mover?.Reset();
        _health?.ResetHealth();
        gameObject.SetActive(true); // ⚠️ ВАЖНО: объект должен быть активен!

        Debug.Log($"[PlayerController] Player active: {gameObject.activeInHierarchy}");
    }

    private void OnJumpPressed() =>
        _mover?.Jump();

    private void OnShootPressed() =>
        _shooter?.Shoot();

    private void OnObstacleHit() =>
        _health?.TakeDamage(Damage);

    private void OnDied()
    {
        _eventBus?.PublishPlayerDied();
        gameObject.SetActive(false);
    }
}