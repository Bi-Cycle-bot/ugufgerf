using Dan.Main;
using Dan.Models;
using TMPro;
using UnityEngine;

namespace Dan.Demo
{
    public class LeaderboardShowcase : MonoBehaviour
    {
        [SerializeField] private string _leaderboardPublicKey = "62c2865869f8faccffacb7e9b34c5addc3c0bff5e1628bfad107ca6629347073";
        
        [SerializeField] private TextMeshProUGUI _playerScoreText;
        [SerializeField] private TextMeshProUGUI[] _entryFields;
        
        [SerializeField] private TMP_InputField _playerUsernameInput;

        private int _playerScore;
        
        private void Start()
        {
            Load();
        }

        public void AddPlayerScore()
        {
            _playerScore++;
            _playerScoreText.text = "Your score: " + _playerScore;
        }
        
        public void Load() => LeaderboardCreator.GetLeaderboard(_leaderboardPublicKey, OnLeaderboardLoaded);

        private void OnLeaderboardLoaded(Entry[] entries)
        {
            foreach (var entryField in _entryFields)
            {
                entryField.text = "";
            }

            for (int i = entries.Length - 1; i >= 0; i--)
            {
                float temp = entries[i].Score;
                temp = temp / 100;
                _entryFields[i].text = $"{i+1}. {entries[i].Username} : {temp}";
            }
        }

        public void Submit()
        {
            float loadedtime = PlayerPrefs.GetFloat("time");
            Debug.Log(loadedtime);
            LeaderboardCreator.UploadNewEntry(_leaderboardPublicKey, _playerUsernameInput.text, (int) (loadedtime * 100) + 1, Callback);
        }
        
        public void DeleteEntry()
        {
            LeaderboardCreator.DeleteEntry(_leaderboardPublicKey, Callback);
        }

        public void ResetPlayer()
        {
            LeaderboardCreator.ResetPlayer();
        }
        
        private void Callback(bool success)
        {
            if (success)
                Load();
        }

        public void loadLeaderboard() {
        LeaderboardCreator.GetLeaderboard(_leaderboardPublicKey, ((msg) => {
            int loopLength = (msg.Length < _entryFields.Length) ? msg.Length : _entryFields.Length;
            for (int i = loopLength - 1; i >= 0; --i) {
                float temp = msg[i].Score;
                temp = temp / 100;
                _entryFields[i].text = $"{i+1}. {msg[i].Username} : {temp}";
            }
        }));
    }
    }
}
