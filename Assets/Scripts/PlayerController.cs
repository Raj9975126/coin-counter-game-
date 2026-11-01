using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

/// <summary>
/// Controls the basket movement left and right, supporting both New Input System and Legacy Input.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Speed of the basket movement")]
    public float moveSpeed = 10f;

    [Tooltip("Horizontal boundary limits (X-axis)")]
    public float xBoundary = 7.5f;

    void Update()
    {
        // Don't move if game is over
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
            return;

        float horizontalInput = 0f;

#if ENABLE_INPUT_SYSTEM
        // New Input System support (Unity 6 / modern Unity)
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                horizontalInput -= 1f;
            }
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                horizontalInput += 1f;
            }
        }
#endif

        // Fallback for Legacy Input System if active
        if (Mathf.Approximately(horizontalInput, 0f))
        {
            try
            {
                horizontalInput = Input.GetAxisRaw("Horizontal");
            }
            catch
            {
                // Legacy input disabled in PlayerSettings, ignore exception
            }
        }

        // Calculate and clamp new position
        Vector3 newPosition = transform.position + Vector3.right * horizontalInput * moveSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, -xBoundary, xBoundary);

        transform.position = newPosition;
    }
}
