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
    public Text HightScoreText;
    public static int ScoreCount;
    public static int HightScoreCount;


    void Start()
    {
        if (PlayerPrefs.HasKey("HightScore"))
        {
            HightScoreCount = PlayerPrefs.GetInt("HiScore");
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (ScoreCount > HightScoreCount)
        {
            HightScoreCount = ScoreCount;
            //HightScoreCount = PlayerPrefs.SetInt("Hi-Score");
        }

        ScoreText.text = "Score" + Mathf.Round(ScoreCount);
        HightScoreText.text = "Hi-Score" + Mathf.Round(HightScoreCount);
    }
    
    public void AddScore(int Points)
    {
        if (_grainReclaimed == true)
        {
            ScoreCount = ScoreCount + 10;
        }

        if (_enemyKilled == true)
        {
            ScoreCount = ScoreCount + 200;
        }

        if (_levelIncreased == true)
        {
            ScoreCount = ScoreCount + 50;
        }
    }

    
}
