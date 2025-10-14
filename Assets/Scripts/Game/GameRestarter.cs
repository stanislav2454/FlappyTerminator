using UnityEngine;

public class GameRestarter : MonoBehaviour
{
    [SerializeField] private EnemyGenerator _enemyGenerator;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private EventBus _eventBus;
    [SerializeField] private BulletPool _bulletPool;
    [SerializeField] private InputHandler _inputHandler;

    private void Awake()
    {
        if (_enemyGenerator == null)
            Debug.LogError("Компонент \"EnemyGenerator\" не установлен в инспекторе!");

        if (_playerController == null)
            Debug.LogError("Компонент \"PlayerController\" не установлен в инспекторе!");

        if (_eventBus == null)
            Debug.LogError("Компонент \"EventBus\" не установлен в инспекторе!");

        if (_bulletPool == null)
            Debug.LogError("Компонент \"BulletPool\" не установлен в инспекторе!");

        if (_inputHandler == null)
            Debug.LogError("Компонент \"InputHandler\" не установлен в инспекторе!");
    }

    private void OnEnable()
    {
        if (_eventBus != null)
            _eventBus.GameRestarted += OnGameRestarted;
    }

    private void OnDisable()
    {
        if (_eventBus != null)
            _eventBus.GameRestarted -= OnGameRestarted;
    }

    private void OnGameRestarted()
    {
        _enemyGenerator?.ResetGenerator();
        _bulletPool?.ResetPool();

        if (_playerController != null)
            _playerController.ResetPlayer();

        _inputHandler?.ResetInput();
    }
}