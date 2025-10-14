using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private int _poolSize = 20;
    [SerializeField] private Transform _container;

    private Queue<Bullet> _pool = new Queue<Bullet>();
    private List<Bullet> _activeBullets = new List<Bullet>();

    private void Awake()
    {
        InitializePool();
    }

    public Bullet GetBullet(Vector3 position, Vector2 direction, BulletOwner owner)
    {
        if (_bulletPrefab == null)
            return null;

        Bullet bullet;

        if (_pool.Count > 0)
            bullet = _pool.Dequeue();
        else
            bullet = CreateNewBullet();

        if (bullet != null)
        {
            bullet.transform.position = position;
            bullet.gameObject.SetActive(true);
            bullet.Initialize(direction, owner);
            _activeBullets.Add(bullet);
        }

        return bullet;
    }

    public void ReturnBullet(Bullet bullet)
    {
        if (bullet == null)
            return;

        bullet.gameObject.SetActive(false);
        _activeBullets.Remove(bullet);
        _pool.Enqueue(bullet);
    }

    public void ResetPool()
    {
        foreach (var bullet in _activeBullets.ToArray())
        {
            if (bullet != null)
                ReturnBullet(bullet);
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            var bullet = CreateNewBullet();

            if (bullet != null)
                _pool.Enqueue(bullet);
        }
    }

    private Bullet CreateNewBullet()
    {
        if (_bulletPrefab == null)
            return null;

        var bullet = Instantiate(_bulletPrefab, _container != null ? _container : transform);
        bullet.SetPool(this);
        bullet.gameObject.SetActive(false);
        return bullet;
    }
}