using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID || UNITY_IOS
using UnityEngine.Advertisements;
#endif

namespace Stairs.Ads
{
    /// <summary>
    /// Handles Unity Ads integration.
    /// </summary>
    public class AdManager : MonoBehaviour
    {
        /// <summary>
        /// Unity project ID for Android platform.
        /// </summary>
        public static string AndroidId = "1228423";

        /// <summary>
        /// Unity project ID for iOS.
        /// </summary>
        public static string IosId = "1228424";

        /// <summary>
        /// Initializes the Ads API integration, if not initialized.
        /// </summary>
        private void Awake()
        {
#if UNITY_ANDROID
            if (Advertisement.isInitialized) return;
            Advertisement.Initialize(AndroidId, true);
#elif UNITY_IOS
            if (Advertisement.isInitialized) return;
            Advertisement.Initialize(AndroidId, true);
#else
            Debug.Log("Ads not supported on this platform!");
#endif
        } 
    }
}