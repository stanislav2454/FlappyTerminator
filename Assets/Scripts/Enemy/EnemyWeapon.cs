using UnityEngine;

public class EnemyWeapon : Weapon
{
    private void Awake()
    {
        _owner = BulletOwner.Enemy;
    }

    public override void Shoot(Vector3 position)
    {
        if (_canShoot == false)
            return;

        Vector2 shootDirection = Vector2.left;
        ProcessShoot(shootDirection, _shootPoint.position);

        if (_cooldownCoroutine != null)
            StopCoroutine(_cooldownCoroutine);

        _cooldownCoroutine = StartCoroutine(CooldownRoutine());
    }
}