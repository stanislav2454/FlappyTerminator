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
        if (_eventBus != null)
            _eventBus.GameRestarted -= OnGameRestarted;
    }

    private void OnGameRestarted()
    {
        _enemyGenerator?.ResetGenerator();

        var bullets = FindObjectsOfType<Bullet>();
        foreach (var bullet in bullets)
            if (bullet != null)
                Destroy(bullet.gameObject);

        _playerController?.ResetPlayer();
    }
}