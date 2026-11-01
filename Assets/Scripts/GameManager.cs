using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    public int score = 0;
    public int maxLives = 3;
    public int currentLives;
    public bool isGameOver = false;

    [Header("TextMeshPro UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    [Header("Standard UI Text Fallback")]
    public Text scoreTextLegacy;
    public Text livesTextLegacy;
    public Text finalScoreTextLegacy;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip coinCatchSound;
    public AudioClip coinMissSound;
    public AudioClip gameOverSound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        currentLives = maxLives;
        isGameOver = false;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        UpdateUI();
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;
        UpdateUI();
        PlaySound(coinCatchSound);
    }

    public void LoseLife()
    {
        if (isGameOver) return;

        currentLives--;
        UpdateUI();
        PlaySound(coinMissSound);

        if (currentLives <= 0)
        {
            TriggerGameOver();
        }
    }

    private void UpdateUI()
    {
        string scoreStr = "Score: " + score;
        if (scoreText != null) scoreText.text = scoreStr;
        if (scoreTextLegacy != null) scoreTextLegacy.text = scoreStr;

        string livesStr = "Lives: " + currentLives;
        if (livesText != null) livesText.text = livesStr;
        if (livesTextLegacy != null) livesTextLegacy.text = livesStr;
    }

    private void TriggerGameOver()
    {
        isGameOver = true;
        PlaySound(gameOverSound);

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        string finalStr = "Final Score: " + score;
        if (finalScoreText != null) finalScoreText.text = finalStr;
        if (finalScoreTextLegacy != null) finalScoreTextLegacy.text = finalStr;
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
