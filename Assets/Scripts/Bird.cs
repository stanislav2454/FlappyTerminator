using System;
using UnityEngine;

[RequireComponent(typeof(BirdMover))]
[RequireComponent(typeof(ScoreCounter))]
[RequireComponent(typeof(BirdCollisionHandler))]
[RequireComponent(typeof(InputHandler))] // ДОБАВИТЬ
public class Bird : MonoBehaviour
{
    [SerializeField] private PlayerShooter _shooter;
    private BirdMover _birdMover;
    private ScoreCounter _scoreCounter;
    private BirdCollisionHandler _handler;
    private InputHandler _inputHandler;

    public event Action GameOver;

    private void Awake()
    {
        _scoreCounter = GetComponent<ScoreCounter>();
        _handler = GetComponent<BirdCollisionHandler>();
        _birdMover = GetComponent<BirdMover>();
        _inputHandler = GetComponent<InputHandler>();

        if (TryGetComponent(out _shooter) == false)
            Debug.LogError("Компонент PlayerShooter не установлен в инспекторе!");
    }

    private void OnEnable()
    {
        _handler.CollisionDetected += ProcessCollision;
        _inputHandler.JumpPressed += OnJumpPressed;
        _inputHandler.ShootPressed += OnShootPressed;
        Bullet.EnemyHit += OnEnemyHit; // ПОДПИСКА НА СОБЫТИЕ ПОПАДАНИЯ
    }

    private void OnDisable()
    {
        _handler.CollisionDetected -= ProcessCollision;
        _inputHandler.JumpPressed -= OnJumpPressed;
        _inputHandler.ShootPressed -= OnShootPressed;
        Bullet.EnemyHit -= OnEnemyHit; // ОТПИСКА ОТ СОБЫТИЯ
    }

    private void OnJumpPressed()
    {
        _birdMover?.Jump();
    }

    private void OnShootPressed()
    {
        _shooter?.Shoot();
    }

    private void OnEnemyHit(int points)
    {    // НОВЫЙ МЕТОД: обработка попадания во врага

        for (int i = 0; i < points; i++)
            _scoreCounter.Add(); // Добавляем очки        
    }

    private void ProcessCollision(IInteractable interactable)
    {
        if (interactable is Enemy)
            GameOver?.Invoke();
        else if (interactable is Bullet bullet)
            GameOver?.Invoke();
        else if (interactable is ScoreZone)
            _scoreCounter.Add();
    }

    public void Reset()
    {
        _scoreCounter.Reset();
        _birdMover.Reset();
    }
}
