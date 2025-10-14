using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletShootPoint;
    [SerializeField] private float _cooldown = 0.5f;

    private bool _canShoot = true;

    private void Awake()
    {
        if (_bulletPrefab == null)
            Debug.LogError("Компонент \"Bullet Prefab\" не установлен в инспекторе!");

        if (_bulletShootPoint == null)
            Debug.LogError("Компонент \"Transform\" для \"bulletShootPoint\" не установлен в инспекторе!");
    }

    public void Shoot()
    {
        if (_canShoot == false || _bulletPrefab == null)
            return;

        Vector2 shootDirection = transform.right;
        Vector3 spawnPosition = new Vector3(_bulletShootPoint.position.x, _bulletShootPoint.position.y, _bulletShootPoint.position.z);

        var bullet = Instantiate(_bulletPrefab, spawnPosition, Quaternion.identity);
        bullet.Initialize(shootDirection, BulletOwner.Player);

        StartCoroutine(CooldownRoutine());
    }

    private System.Collections.IEnumerator CooldownRoutine()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_cooldown);
        _canShoot = true;
    }
}