using UnityEngine;
using System;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 1;

    private int _currentHealth;

    public event Action Died;
    public event Action<int> HealthChanged;

    private void Start() =>
        ResetHealth();

    public void TakeDamage(int damage)
    {
        if (_currentHealth <= 0)
            return;

        _currentHealth = Mathf.Max(0, _currentHealth - damage);
        HealthChanged?.Invoke(_currentHealth);

        if (_currentHealth <= 0)
            Died?.Invoke();
    }

    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
        HealthChanged?.Invoke(_currentHealth);
    }
}