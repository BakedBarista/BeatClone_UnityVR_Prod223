using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    public TMP_Text finalScoreText;
    public TMP_Text streakText;

    void Start()
    {
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        int streak = PlayerPrefs.GetInt("Streak", 0);
        finalScoreText.text = finalScore.ToString();
        streakText.text = streak.ToString();
    }
}
