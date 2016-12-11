using System.Collections;
using System.Collections.Generic;
using Stairs.Utils;
using UnityEngine;
#if UNITY_ANDROID || UNITY_IOS
using UnityEngine.Advertisements;
#endif
using UnityEngine.UI;

namespace Stairs.GUI
{
    /// <summary>
    /// Handles the dialog offering player save when missing a step.
    /// </summary>
    public class SaveDialog : MonoBehaviour
    {
        /// <summary>
        /// Textfield on the button.
        /// </summary>
        [SerializeField] private Text ButtonText;

        /// <summary>
        /// How many coins would the save cost.
        /// </summary>
        [SerializeField] private int SaveCost = 10;

        /// <summary>
        /// String template for saving wiht coins.
        /// </summary>
        [SerializeField] private string MessageSave = "Save for {0} coins!";

        /// <summary>
        /// String template for IAP call to action, in case player has no coins.
        /// </summary>
        [SerializeField] private string MessageBuy = "Buy coins to save!";

        /// <summary>
        /// True if player can affor the save.
        /// </summary>
        private bool CanAffordSave {  get { return DataStorage.Instance.Coins >= SaveCost; } }

        /// <summary>
        /// Initializes the dialog. Called whem player misses a step.
        /// </summary>
        public void OnSaveChance()
        {
            gameObject.SetActive(true);
            ButtonText.text = string.Format( CanAffordSave ? MessageSave : MessageBuy, SaveCost);
        }

        /// <summary>
        /// Public function for accepting the offer or taking the IAP offer to get more coins.
        /// </summary>
        public void OnButtonClick()
        {
            if (DataStorage.Instance.PayCoins(SaveCost)) PlayerController.DieOnMiss = false;
            OnOfferPass();
        }

        /// <summary>
        /// If player cancels and accepts death.
        /// </summary>
        public void OnOfferPass()
        {
            Pool.Instance.SceneControl.SaveDialogOpen = false;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// If the player wishes to save by watching an ad.
        /// </summary>
        public void OnAdSaveButtonClick()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (Advertisement.IsReady("rewardedVideo") && !Advertisement.isShowing)
            {
                Advertisement.Show("rewardedVideo", new ShowOptions { resultCallback = HandleClosingOfAd });
            }
#endif
        }
#if UNITY_ANDROID || UNITY_IOS
        /// <summary>
        /// Handles the asyncronous callback of ad playback.
        /// </summary>
        /// <param name="result">Result of the ad playback.</param>
        private void HandleClosingOfAd(ShowResult result)
        {
            // Showresult.Finished is returned only if the player did not
            // prematurely quit the ad playback.
            if (result == ShowResult.Finished)
            {
                PlayerController.DieOnMiss = false;
                OnOfferPass();
            }
        }
#endif
    }
}