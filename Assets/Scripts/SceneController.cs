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
        [SerializeField] private string ScoreTextPattern = "{0} Steps";
        [SerializeField] private Text CoinText;
        [SerializeField] private string CoinPattern = "Coins {0}";

        public void IncreasePlayerScore()
        {
            ScoreText.text = string.Format(ScoreTextPattern, DataStorage.Instance.AddToSteps(1));
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
            CoinText.text = string.Format(CoinPattern, DataStorage.Instance.AddToCoins(1));
        }
    }
}
