using UnityEngine;

public class PlayerWeapon : Weapon
{
    [SerializeField] private Transform _shootPoint;

    private void Awake()
    {
        _owner = BulletOwner.Player;
    }

    public override void Shoot(Vector3 position)
    {
        if (_canShoot == false)
            return;

        Vector2 shootDirection = transform.right;
        ProcessShoot(shootDirection, _shootPoint.position);

        if (_cooldownCoroutine != null)
            StopCoroutine(_cooldownCoroutine);

        _cooldownCoroutine = StartCoroutine(CooldownRoutine());
    }
}