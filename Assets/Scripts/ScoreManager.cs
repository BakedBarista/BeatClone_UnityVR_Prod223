using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public TMP_Text scoreText;

    private int score = 0;
    private int streak = 0;
    private int highStreak = 0;
    private int multiplier = 1;
    private const int maxMultiplier = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void AddScore()
    {
        streak++;
        if (streak >= highStreak)
        {
            highStreak = streak;
        }
        if (streak % 5 == 0)
        {
            multiplier = Mathf.Min(multiplier + 1, maxMultiplier);
        }

        score += 5 * multiplier;
        UpdateScoreText();
    }

    public void ResetStreak()
    {
        streak = 0;
        multiplier = 1;
        UpdateScoreText ();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score + "\n" + "X" + multiplier;
        }
    }

    public void EndGame()
    {
        PlayerPrefs.SetInt("FinalScore", score);
        PlayerPrefs.SetInt("Streak", highStreak);
        SceneManager.LoadScene("Scoreboard");
    }
}
