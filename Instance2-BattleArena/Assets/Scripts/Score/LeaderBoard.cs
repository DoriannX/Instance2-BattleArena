using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using Dan.Main;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> _names;
    [SerializeField]
    private List<TextMeshProUGUI> _scores;

    private string _publicLeaderBoardKey =
        "b3281035df4bdb48df8a1842c0f28cf8f0560d46d66d0f29acc5c2a0366c5aef";

    public void GetLeaderBoard()
    {
        LeaderboardCreator.GetLeaderboard(_publicLeaderBoardKey, ((msg) =>
        {
              
            for (int i = 0; i < _names.Count; ++i)
            {
                _names[i].text = msg[i].Username;
                _scores[i].text = msg[i].Score.ToString();
            }

        }));
    }

    public void SetLeaderBoardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(_publicLeaderBoardKey, username, 
            score, ((msg) =>
            {
            username.Substring(0, 4);
            GetLeaderBoard();
            }));
    }


}
