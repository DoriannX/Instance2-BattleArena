using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private bool _levelIncreased;
    private bool _grainReclaimed;
    private bool _enemyKilled;
    public Text ScoreText;
    public Text HighScoreText;
    public static int ScoreCount;
    public static int HighScoreCount;

    void Start()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            HighScoreCount = PlayerPrefs.GetInt("HighScore");
        }
    }

    void Update()
    {
        if (ScoreCount > HighScoreCount)
        {
            HighScoreCount = ScoreCount;
            PlayerPrefs.SetInt("HighScore", HighScoreCount);
        }

        ScoreText.text = $"Score: {Mathf.Round(ScoreCount)}";
        HighScoreText.text = $"High Score: {Mathf.Round(HighScoreCount)}";
    }

    public void AddScore(int points)
    {
        ScoreCount += points;
        if (ScoreCount > HighScoreCount)
        {
            HighScoreCount = ScoreCount;
            PlayerPrefs.SetInt("HighScore", HighScoreCount);
        }
    }
}