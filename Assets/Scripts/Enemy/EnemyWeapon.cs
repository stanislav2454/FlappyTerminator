using UnityEngine;

public class EnemyWeapon : Weapon
{
    private void Awake()
    {
        _owner = BulletOwner.Enemy;
        Debug.Log($"🔫 EnemyWeapon Awake: {gameObject.name}, BulletPool: {_bulletPool != null}");
    }

    public override void Shoot(Vector3 position)
    {
        Debug.Log($"🔫 EnemyWeapon Shoot called: {gameObject.name}, CanShoot: {_canShoot}");
        if (_canShoot == false)
        {
            Debug.Log($"🔫 EnemyWeapon: Cooldown active");
            return;
        }

        Vector2 shootDirection = Vector2.left;
        Debug.Log($"🔫 EnemyWeapon: Processing shoot at {position}");
        ProcessShoot(shootDirection, position);

        if (_cooldownCoroutine != null)
            StopCoroutine(_cooldownCoroutine);

        _cooldownCoroutine = StartCoroutine(CooldownRoutine());
    }
}