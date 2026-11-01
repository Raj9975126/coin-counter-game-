using UnityEngine;

/// <summary>
/// Handles coin falling, catching detection, and missing detection.
/// </summary>
public class Coin : MonoBehaviour
{
    [Header("Coin Settings")]
    [Tooltip("Downward falling speed")]
    public float fallSpeed = 5f;

    [Tooltip("Points awarded when caught")]
    public int scoreValue = 1;

    [Tooltip("Y position threshold where coin is considered missed")]
    public float bottomLimit = -5.5f;

    void Update()
    {
        // Don't fall if game is over
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
            return;

        // Move the coin downward every frame
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // Check if coin went past the bottom of the screen (Missed Coin)
        if (transform.position.y < bottomLimit)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoseLife();
            }
            Destroy(gameObject);
        }
    }

    // Trigger collision detection with the basket
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ensure player object has the tag "Player"
        if (collision.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(scoreValue);
            }
            Destroy(gameObject);
        }
    }
}
