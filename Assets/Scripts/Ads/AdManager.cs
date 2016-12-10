using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Stairs.Ads
{
    public class AdManager : MonoBehaviour
    {
        public static string AndroidId = "1228423";
        public static string IosId = "1228424";

        private void Awake()
        {
#if UNITY_ANDROID
            if (Advertisement.isInitialized) return;
            Advertisement.Initialize(AndroidId, true);
#elif UNITY_IOS
            if (Advertisement.isInitialized) return;
            Advertisement.Initialize(AndroidId, true);
#else
            Debug.Log("Ads not supported on this platform!")
#endif
        } 
    }
}