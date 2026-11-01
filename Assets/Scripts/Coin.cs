using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Settings")]
    public float fallSpeed = 5f;
    public int scoreValue = 1;
    public float bottomLimit = -5.5f;

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
            return;

        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        if (transform.position.y < bottomLimit)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoseLife();
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
