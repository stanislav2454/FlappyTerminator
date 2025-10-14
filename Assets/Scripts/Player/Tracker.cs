using UnityEngine;

public class Tracker : MonoBehaviour
{
    [SerializeField] private PlayerController _plane;
    [SerializeField] private float _xOffset;

    private void Update()
    {
        var position = transform.position;
        position.x = _plane.transform.position.x + _xOffset;
        transform.position = position;
    }
}