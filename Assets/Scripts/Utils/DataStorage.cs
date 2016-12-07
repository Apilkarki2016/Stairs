using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stairs.Utils
{
    public class DataStorage : Singleton<DataStorage>
    {
        public int StepsCurrent;
        public int CoinsCurrent;
        public int GemsCurrent;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void EndOfPlayLoop()
        {
            
        }

        public void InitializeForPlayloop()
        {
            StepsCurrent = 0;
            CoinsCurrent = 0;
            GemsCurrent = 0;
        }

        public int AddToSteps(int howMuch = 1)
        {
            if (howMuch > 0) StepsCurrent += howMuch;
            return StepsCurrent;;
        }

        public int AddToCoins(int howMuch = 1)
        {
            if (howMuch > 0) CoinsCurrent += howMuch;
            return CoinsCurrent; ;
        }

        public int AddToGems(int howMuch = 1)
        {
            if (howMuch > 0) GemsCurrent += howMuch;
            return GemsCurrent; ;
        }

        public bool PayCoins(int howMany)
        {
            return false;
        }

        public bool PayGems(int howMany)
        {
            return false;
        }
    }
}