using UnityEngine;

public class BulletPool : GenericPool<Bullet>
{
    public Bullet GetBullet(Vector3 position, Vector2 direction, BulletOwner owner)
    {
        var bullet = GetObject(position);
        bullet?.Initialize(direction, owner);

        return bullet;
    }

    public void ReturnBullet(Bullet bullet) =>
        ReturnObject(bullet);

    protected override Bullet CreateNewObject()
    {
        var bullet = base.CreateNewObject();

        if (bullet != null)
            bullet.ReturnedToPool += ReturnBullet;

        return bullet;
    }

    protected virtual void OnDestroy()
    {
        foreach (var bullet in _activeObjects)
            if (bullet != null)
                bullet.ReturnedToPool -= ReturnBullet;

        foreach (var bullet in _pool)
            if (bullet != null)
                bullet.ReturnedToPool -= ReturnBullet;
    }
}