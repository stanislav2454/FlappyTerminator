using UnityEngine;

public class GameRestarter : MonoBehaviour
{
    [SerializeField] private EnemyGenerator _enemyGenerator;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private BulletPool _bulletPool;
    [SerializeField] private InputHandler _inputHandler;

    private void Awake()
    {
        if (_enemyGenerator == null)
            Debug.LogError("Компонент \"EnemyGenerator\" не установлен в инспекторе!");

        if (_playerController == null)
            Debug.LogError("Компонент \"PlayerController\" не установлен в инспекторе!");

        if (_gameManager == null)
            Debug.LogError("Компонент \"GameManager\" не установлен в инспекторе!");

        if (_bulletPool == null)
            Debug.LogError("Компонент \"BulletPool\" не установлен в инспекторе!");

        if (_inputHandler == null)
            Debug.LogError("Компонент \"InputHandler\" не установлен в инспекторе!");
    }

    private void OnEnable()
    {
        if (_gameManager != null)
            _gameManager.GameRestarted += OnGameRestarted;
    }

    private void OnDisable()
    {
        if (_gameManager != null)
            _gameManager.GameRestarted -= OnGameRestarted;
    }

    public void RestartGame()
    {
        _enemyGenerator?.ResetGenerator();
        _bulletPool?.ResetPool();

        if (_playerController != null)
            _playerController.ResetPlayer();

        _inputHandler?.EnableInput();
    }

    private void OnGameRestarted()
    {
        RestartGame();
    }
}