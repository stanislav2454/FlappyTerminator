// Enemy.cs
using UnityEngine;

public class Enemy : MonoBehaviour, IInteractable
{
    [SerializeField] private float _shootDelay = 2f;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _movementAmplitude = 1f;
    [SerializeField] private float _movementFrequency = 1f;

    private Vector3 _startPosition;
    private float _time;

    private void Start()
    {
        _startPosition = transform.position;
        StartCoroutine(ShootingRoutine());
    }

    private void Update()
    {  // Движение вверх-вниз для усложнения игры
        _time += Time.deltaTime;
        float newY = _startPosition.y + Mathf.Sin(_time * _movementFrequency) * _movementAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private System.Collections.IEnumerator ShootingRoutine()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(_shootDelay);
            Shoot();
        }
    }

    private void Shoot()
    {
        var bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        bullet.Initialize(Vector2.left, BulletOwner.Enemy); // Стреляют влево
    }
}