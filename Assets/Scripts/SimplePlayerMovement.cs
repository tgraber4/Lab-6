using UnityEngine;
using UnityEngine.InputSystem;
public class SimplePlayerMovement : MonoBehaviour
{
    public float moveSpeed = 4f;
    void Update()
    {
        if (Keyboard.current == null)
            return;
        float horizontal = 0f;
        if (Keyboard.current.aKey.isPressed)
            horizontal = -1f;
        if (Keyboard.current.dKey.isPressed)
            horizontal = 1f;
        transform.Translate(
        Vector3.right * horizontal *
        moveSpeed * Time.deltaTime,
        Space.World
        );
    }
}