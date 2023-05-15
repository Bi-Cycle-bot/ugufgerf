using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> names;
    [SerializeField] private List<TextMeshProUGUI> scores;

    private string publicLeaderboardKey = "62c2865869f8faccffacb7e9b34c5addc3c0bff5e1628bfad107ca6629347073";

    void Start() {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, (msg) => {});
    }

    /*public void GetLeaderboard() {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) => {
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i < loopLength; ++i) {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Score.ToString();
            }
        }));
    }*/

    public void SetLeaderboardEntry(string username, int score) {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score, ((_msg) => {
            LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, (msg) => {});
        }));
    }
}
