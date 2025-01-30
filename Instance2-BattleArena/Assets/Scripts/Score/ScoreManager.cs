using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    private bool _levelIncreased;
    private bool _grainReclaimed;
    private bool _enemyKilled;
    public Text ScoreText;
    public Text HighScoreText;
    public static int ScoreCount;
    public static int HiScoreCount;

    [SerializeField]
    private TextMeshProUGUI inputScore;
    [SerializeField]
    private TMP_InputField inputName;

    public UnityEvent<string, int> submitScoreEvent;


    void Start()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            HiScoreCount = PlayerPrefs.GetInt("HiScore");
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (ScoreCount > HiScoreCount)
        {
            HiScoreCount = ScoreCount;
            PlayerPrefs.SetInt("Hi-Score", HiScoreCount);
        }

        //ScoreText.text = "Score: " + ScoreCount;
       // HighScoreText.text = "Hi-Score: " + HiScoreCount;
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

    public void SubmitScore()
    {
        submitScoreEvent.Invoke(inputName.text, int.Parse(inputScore.text));

    }

    
}
