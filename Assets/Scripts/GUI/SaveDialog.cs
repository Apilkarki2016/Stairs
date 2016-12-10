using System.Collections;
using System.Collections.Generic;
using Stairs.Utils;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace Stairs.GUI
{
    public class SaveDialog : MonoBehaviour
    {
        [SerializeField] private Text ButtonText;
        [SerializeField] private int SaveCost = 10;
        [SerializeField] private string MessageSave = "Save for {0} coins!";
        [SerializeField] private string MessageBuy = "Buy coins to save!";

        private bool CanAffordSave {  get { return DataStorage.Instance.Coins >= SaveCost; } }

        public void OnSaveChance()
        {
            gameObject.SetActive(true);
            ButtonText.text = string.Format( CanAffordSave ? MessageSave : MessageBuy, SaveCost);
        }

        public void OnButtonClick()
        {
            if (DataStorage.Instance.PayCoins(SaveCost)) PlayerController.DieOnMiss = false;
            OnOfferPass();
        }

        public void OnOfferPass()
        {
            Pool.Instance.SceneControl.SaveDialogOpen = false;
            gameObject.SetActive(false);
        }

        public void OnAdSaveButtonClick()
        {
            if (Advertisement.IsReady("rewardedVideo") && !Advertisement.isShowing)
            {
                Advertisement.Show("rewardedVideo", new ShowOptions { resultCallback = HandleClosingOfAd });
            }
        }

        private void HandleClosingOfAd(ShowResult result)
        {
            Debug.Log(result.ToString());
            if (result == ShowResult.Finished)
            {
                PlayerController.DieOnMiss = false;
                OnOfferPass();
            }
        }
    }
}