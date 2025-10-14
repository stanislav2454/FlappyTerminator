using UnityEngine;
using System;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private EventBus _eventBus;

    public event Action JumpPressed;
    public event Action ShootPressed;

    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _shootKey = KeyCode.X;

    private bool _inputEnabled = false;

    private void Awake()
    {
        if (_eventBus == null)
            Debug.LogError("Компонент \"EventBus\" не установлен в инспекторе!");
    }

    private void OnEnable()
    {
        if (_eventBus != null)
        {
            _eventBus.GameStarted += OnGameStarted;
            _eventBus.GameRestarted += OnGameRestarted;
            _eventBus.PlayerDied += OnPlayerDied;
        }
    }

    private void OnDisable()
    {
        if (_eventBus != null)
        {
            _eventBus.GameStarted -= OnGameStarted;
            _eventBus.GameRestarted -= OnGameRestarted;
            _eventBus.PlayerDied -= OnPlayerDied;
        }
    }

    private void Update()
    {
        if (_inputEnabled == false)
            return;

        if (Input.GetKeyDown(_jumpKey))
            JumpPressed?.Invoke();

        if (Input.GetKeyDown(_shootKey))
            ShootPressed?.Invoke();
    }

    public void ResetInput()
    {
        _inputEnabled = true;
        StopAllCoroutines();
    }

    private void OnGameStarted() =>
        _inputEnabled = true;

    private void OnGameRestarted() =>
        _inputEnabled = true;

    private void OnPlayerDied() =>
        _inputEnabled = false;
}