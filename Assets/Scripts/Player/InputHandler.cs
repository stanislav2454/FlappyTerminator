using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public event Action JumpPressed;
    public event Action ShootPressed;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            JumpPressed?.Invoke();

        if (Input.GetKeyDown(KeyCode.LeftControl))
            ShootPressed?.Invoke();
    }
}