using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Stairs.Utils
{
    public class SceneController : MonoBehaviour
    {
        public bool SaveDialogOpen = true;

        [SerializeField] private Text ScoreText;
        [SerializeField] private string ScoreTextPattern = "{0} Steps";
        [SerializeField] private Text CoinText;
        [SerializeField] private string CoinPattern = "Coins {0}";

        private AudioSource _coinAudio;
        private AudioSource _musicAudio;

        private void Update()
        {
            ScoreText.text = string.Format(ScoreTextPattern, DataStorage.Instance.CurrentScore);
            CoinText.text = string.Format(CoinPattern, DataStorage.Instance.Coins);
        }

        private void Awake()
        {
            var sources = GetComponents<AudioSource>();
            _musicAudio = sources[0];
            _coinAudio = sources[1];
        }

        public void IncreasePlayerScore(int howMany)
        {
            DataStorage.Instance.AddToScore(howMany);
        }

        public void ReloadScene()
        {
            DataStorage.Instance.EndOfPlayLoop();
            SceneManager.LoadScene(0);
            Pool.Instance.DestroyAllPools();
            Pool.Instance.ReInitialize();
            DataStorage.Instance.InitializeForPlayloop();
        }

        public void CollectCoin()
        {
            _coinAudio.PlayOneShot(_coinAudio.clip);
            DataStorage.Instance.AddToCoins(1);
        }

        public void MusicSetPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                _musicAudio.Pause();
            }
            else
            {
                _musicAudio.Play();
            }
        }
    }
}
