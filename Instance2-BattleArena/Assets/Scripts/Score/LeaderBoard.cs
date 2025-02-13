using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Dan.Main;


public class LeaderBoard : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> _names;
    [SerializeField]
    private List<TextMeshProUGUI> _scores;

    private string _publicLeaderBoardKey =
        "b2e20afac89b0a140bf70081ff72b0c69bb5a9fa3c2ff6278e1536ab7187f5ca";



    public void GetLeaderBoard()
    {
        LeaderboardCreator.GetLeaderboard(_publicLeaderBoardKey, ((msg) =>
        {
              
            for (int i = 0; i < _names.Count; ++i)
            {
                string username = msg[i].Username;
                _names[i].text = username;
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
