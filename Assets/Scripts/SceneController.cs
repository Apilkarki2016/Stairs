using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Stairs.Utils
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private Text ScoreText;
        [SerializeField] private string ScoreTextPatter = "{0} Steps!";

        private int _playerScore = 0;

        public void IncreasePlayerScore()
        {
            _playerScore++;
            ScoreText.text = string.Format(ScoreTextPatter, _playerScore);
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(0);
            Pool.Instance.DestroyAllPools();
            Pool.Instance.ReInitialize();
        }
    }
}
