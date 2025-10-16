using UnityEngine;
using System.Collections;

public class PerformanceMonitor : MonoBehaviour
{
    private int _totalBulletsCreated = 0;
    private int _totalBulletsFromPool = 0;

    private void Start()
    {
        StartCoroutine(LogPerformance());
    }

    private IEnumerator LogPerformance()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            // Найти все пули в сцене
            var allBullets = FindObjectsOfType<Bullet>();
            var allEnemies = FindObjectsOfType<Enemy>();
            var allWeapons = FindObjectsOfType<Weapon>();
            var bulletPool = FindObjectOfType<BulletPool>();

            Debug.Log($"=== PERFORMANCE REPORT ===");
            Debug.Log($"FPS: {1f / Time.deltaTime:F1}");
            Debug.Log($"Enemies in scene: {allEnemies.Length}");
            Debug.Log($"Weapons in scene: {allWeapons.Length}");
            Debug.Log($"Total Bullets in scene: {allBullets.Length}");
            Debug.Log($"Bullets from pool: {_totalBulletsFromPool}");
            Debug.Log($"Bullets created new: {_totalBulletsCreated}");

            // ✅ ИСПРАВЛЕННЫЕ МЕТОДЫ - используем существующие в GenericPool
            if (bulletPool != null)
            {
                // Используем рефлексию чтобы получить protected поля
                var poolType = bulletPool.GetType();
                var activeField = poolType.GetField("_activeObjects",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var poolField = poolType.GetField("_pool",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (activeField != null && poolField != null)
                {
                    var activeObjects = activeField.GetValue(bulletPool) as System.Collections.IList;
                    var poolObjects = poolField.GetValue(bulletPool) as System.Collections.ICollection;

                    Debug.Log($"BulletPool active: {activeObjects?.Count ?? 0}");
                    Debug.Log($"BulletPool pooled: {poolObjects?.Count ?? 0}");
                }
            }
            else
            {
                Debug.Log($"BulletPool: NOT FOUND IN SCENE!");
            }

            if (_totalBulletsCreated > 10)
            {
                Debug.LogError($"🚨 CRITICAL: {_totalBulletsCreated} NEW BULLETS CREATED!");
            }

            // ✅ ДИАГНОСТИКА ВРАГОВ
            foreach (var enemy in allEnemies)
            {
                var weapon = enemy.GetComponent<EnemyWeapon>();
                Debug.Log($"Enemy: {enemy.name}, Weapon: {weapon != null}, Active: {enemy.gameObject.activeInHierarchy}");
            }

            if (_totalBulletsCreated > 10)
            {
                Debug.LogError($"🚨 CRITICAL: {_totalBulletsCreated} NEW BULLETS CREATED!");
            }

            Debug.Log($"==========================");
        }
    }

    // Вызывать из Weapon когда создается новая пуля
    public void BulletCreated(bool fromPool)
    {
        if (fromPool)
            _totalBulletsFromPool++;
        else
            _totalBulletsCreated++;
    }
}