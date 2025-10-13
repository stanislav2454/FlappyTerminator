using UnityEngine;

public class EnemyRemover : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.TryGetComponent(out Enemy enemy))
        {
            Destroy(other.gameObject);
        }
    }
}