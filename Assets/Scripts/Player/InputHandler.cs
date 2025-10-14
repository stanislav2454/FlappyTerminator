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
            _eventBus = FindObjectOfType<EventBus>();

        Debug.Log($"[InputHandler] Awake called. Active: {this.enabled}, GameObject: {gameObject.activeInHierarchy}");
    }

    private void Start()
    {
        Debug.Log($"[InputHandler] Start called. _eventBus: {_eventBus != null}");
    }

    private void OnEnable()
    {
        Debug.Log("[InputHandler] OnEnable called - subscribing to events");
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
        // Временная принудительная активация для тестирования
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("[InputHandler] MANUAL ENABLE via F2");
            _inputEnabled = true;
        }
        if (_inputEnabled == false)
        {
            Debug.Log($"[InputHandler] Input disabled in Update. _inputEnabled = {_inputEnabled}");
            return;
        }

        if (Input.GetKeyDown(_jumpKey))
        {
            JumpPressed?.Invoke();
            Debug.Log("[InputHandler] Jump pressed");
        }

        if (Input.GetKeyDown(_shootKey))
        {
            ShootPressed?.Invoke();
            Debug.Log("[InputHandler] Shoot pressed");
        }
    }

    private void OnGameStarted()
    {
        _inputEnabled = true;
        Debug.Log("[InputHandler] Input enabled");
    }

    private void OnGameRestarted()
    {
        _inputEnabled = true;
        Debug.Log($"[InputHandler] Input enabled (restart). _inputEnabled = {_inputEnabled}");

        // Дополнительная проверка
        Debug.Log($"[InputHandler] Game object active: {gameObject.activeInHierarchy}");
        Debug.Log($"[InputHandler] Component enabled: {this.enabled}");
    }

    private void OnPlayerDied()
    {
        _inputEnabled = false;
        Debug.Log("[InputHandler] Input disabled");
    }
}