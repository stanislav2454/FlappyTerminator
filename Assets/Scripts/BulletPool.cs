using UnityEngine;

public class BulletPool : GenericPool<Bullet>
{
    public Bullet GetBullet(Vector3 position, Vector2 direction, BulletOwner owner)
    {
        var bullet = GetObject(position);

        bullet?.Initialize(direction, owner);

        return bullet;
    }

    protected override Bullet CreateNewObject()
    {
        var bullet = base.CreateNewObject();

        bullet?.SetPool(this);

        return bullet;
    }

    public void ReturnBullet(Bullet bullet) =>
        ReturnObject(bullet);
}