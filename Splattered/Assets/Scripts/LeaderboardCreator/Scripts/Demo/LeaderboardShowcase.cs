using Dan.Main;
using Dan.Models;
using TMPro;
using UnityEngine;

namespace Dan.Demo
{
    public class LeaderboardShowcase : MonoBehaviour
    {
        [SerializeField] private string _leaderboardPublicKey = "62c2865869f8faccffacb7e9b34c5addc3c0bff5e1628bfad107ca6629347073";
        
        [SerializeField] private TextMeshProUGUI _playerTimeText;
        [SerializeField] private TextMeshProUGUI[] _entryFields;
        
        [SerializeField] private TMP_InputField _playerUsernameInput;

        private float _playerTime;
        
        private void Start()
        {
            _playerTime = PlayerPrefs.GetFloat("time");
            AddPlayerScore();
            Load();
        }

        public void AddPlayerScore()
        {
            _playerTimeText.text = "Your score: " + _playerTime;
        }
        
        public void Load() => LeaderboardCreator.GetLeaderboard(_leaderboardPublicKey, OnLeaderboardLoaded);

        private void OnLeaderboardLoaded(Entry[] entries)
        {
            foreach (var entryField in _entryFields)
            {
                entryField.text = "";
            }

            for (int i = 0; i < entries.Length; i++)
            {
                float temp = entries[i].Score;
                temp = -1 * temp / 100;
                _entryFields[i].text = $"{i+1}. {entries[i].Username} : {temp}";
            }
        }

        public void Submit()
        {
            LeaderboardCreator.UploadNewEntry(_leaderboardPublicKey, _playerUsernameInput.text, (int) ((-1 * ((_playerTime * 100) + 1))), Callback);
        }
        
        public void DeleteEntry()
        {
            LeaderboardCreator.DeleteEntry(_leaderboardPublicKey, Callback);
        }
        
        private void Callback(bool success)
        {
            if (success)
                Load();
        }
    }
}
