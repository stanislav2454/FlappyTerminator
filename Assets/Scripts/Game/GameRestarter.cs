using UnityEngine;

public class GameRestarter : MonoBehaviour
{
    [SerializeField] private EnemyGenerator _enemyGenerator;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private EventBus _eventBus;

    private void Awake()
    {
        if (_enemyGenerator == null)
            Debug.LogError("Компонент \"EnemyGenerator\" не установлен в инспекторе!");

        if (_playerController == null)
            Debug.LogError("Компонент \"PlayerController\" не установлен в инспекторе!");

        if (_eventBus == null)
            Debug.LogError("Компонент \"EventBus\" не установлен в инспекторе!");
    }

    private void OnEnable()
    {
        if (_eventBus != null)
            _eventBus.GameRestarted += OnGameRestarted;
    }

    private void OnDisable()
    {
        _eventBus.GameRestarted -= OnGameRestarted; // ✅ ОТПИСКА активна
        Debug.Log("[GameRestarter] Unsubscribed from GameRestarted event");
    }

    private void OnGameRestarted()
    {
        Debug.Log("[GameRestarter] Starting game restart...");

        // 1. Очистить врагов
        _enemyGenerator?.ResetGenerator();

        // 2. Очистить пули
        var bullets = FindObjectsOfType<Bullet>();
        foreach (var bullet in bullets)
        {
            if (bullet != null)
                Destroy(bullet.gameObject);
        }

        // 3. Сбросить игрока (ВАЖНО!)
        if (_playerController != null)
        {
            Debug.Log("[GameRestarter] Resetting player controller");
            _playerController.ResetPlayer();
        }
        //else
        //{
        //    Debug.LogError("[GameRestarter] PlayerController reference is null!");
        //}   
        // 4. ✅ ПРИНУДИТЕЛЬНО ВКЛЮЧИТЬ ВВОД
        var inputHandler = FindObjectOfType<InputHandler>();
        if (inputHandler != null)
        {
            Debug.Log("[GameRestarter] Force enabling input via reflection");
            // Через рефлексию принудительно вызываем OnGameRestarted
            var method = typeof(InputHandler).GetMethod("OnGameRestarted",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method?.Invoke(inputHandler, null);
        }
        Debug.Log("[GameRestarter] Game restart completed");
    }
}