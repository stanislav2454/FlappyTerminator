using UnityEngine;
using System;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _shootKey = KeyCode.X;

    private bool _inputEnabled = false;

    public event Action JumpPressed;
    public event Action ShootPressed;

    private void Awake()
    {
        if (_gameManager == null)
            Debug.LogError("Компонент \"GameManager\" не установлен в инспекторе!");
    }

    private void OnEnable()
    {
        if (_gameManager != null)
        {
            _gameManager.GameStarted += OnGameStarted;
            _gameManager.GameRestarted += OnGameRestarted;
            _gameManager.PlayerDied += OnPlayerDied;
        }
    }

    private void OnDisable()
    {
        if (_gameManager != null)
        {
            _gameManager.GameStarted -= OnGameStarted;
            _gameManager.GameRestarted -= OnGameRestarted;
            _gameManager.PlayerDied -= OnPlayerDied;
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

    public void EnableInput() =>
        _inputEnabled = true;

    public void DisableInput() =>
        _inputEnabled = false;

    private void OnGameStarted() =>
        EnableInput();

    private void OnGameRestarted() =>
        EnableInput();

    private void OnPlayerDied() =>
        DisableInput();
}