using UnityEngine;

public interface IBullet
{
    void Initialize(Vector2 direction, BulletOwner owner);
}