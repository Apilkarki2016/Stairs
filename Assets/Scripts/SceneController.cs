using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Stairs.Utils
{
    /// <summary>
    /// Handles misc functions in running scene.
    /// </summary>
    public class SceneController : MonoBehaviour
    {
        /// <summary>
        /// Is the player save dialog open?
        /// </summary>
        /// <remarks>
        /// Asyncronously closing the dialog and proceeding on waiting scripts depend on
        /// this value.
        /// </remarks>
        public bool SaveDialogOpen = false;

        /// <summary>
        /// Text field for score.
        /// </summary>
        [SerializeField] private Text ScoreText;

        /// <summary>
        /// String pattern for score text.
        /// </summary>
        [SerializeField] private string ScoreTextPattern = "{0} Steps";

        /// <summary>
        /// Text field for coins.
        /// </summary>
        [SerializeField] private Text CoinText;

        /// <summary>
        /// String pattern for coins.
        /// </summary>
        [SerializeField] private string CoinPattern = "Coins {0}";

        /// <summary>
        /// Audiosoure for coin pickup.
        /// </summary>
        private AudioSource _coinAudio;

        /// <summary>
        /// Audiosource for music playback.
        /// </summary>
        private AudioSource _musicAudio;

        /// <summary>
        /// Update GUI texts.
        /// </summary>
        private void Update()
        {
            ScoreText.text = string.Format(ScoreTextPattern, DataStorage.Instance.CurrentScore);
            CoinText.text = string.Format(CoinPattern, DataStorage.Instance.Coins);
        }

        /// <summary>
        /// Initializes audiosources.
        /// </summary>
        private void Awake()
        {
            var sources = GetComponents<AudioSource>();
            _musicAudio = sources[0];
            _coinAudio = sources[1];
        }

        /// <summary>
        /// Increases players score.
        /// </summary>
        /// <param name="howMany">A positive integer.</param>
        public void IncreasePlayerScore(int howMany)
        {
            DataStorage.Instance.AddToScore(howMany);
        }

        /// <summary>
        /// Reloads the scene and reinitializes as needed.
        /// </summary>
        public void ReloadScene()
        {
            DataStorage.Instance.EndOfPlayLoop();
            SceneManager.LoadScene(0);
            Pool.Instance.DestroyAllPools();
            Pool.Instance.ReInitialize();
            DataStorage.Instance.InitializeForPlayloop();
        }

        /// <summary>
        /// Handles player collecting a coin.
        /// </summary>
        public void CollectCoin()
        {
            _coinAudio.PlayOneShot(_coinAudio.clip);
            DataStorage.Instance.AddToCoins(1);
        }

        /// <summary>
        /// Toggles music playback.
        /// </summary>
        /// <param name="pauseStatus">True to pause, false to resume.</param>
        /// <remarks>
        /// Called by various UnityEvents.
        /// </remarks>
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
