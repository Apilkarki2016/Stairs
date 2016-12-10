using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stairs.Utils
{
    public partial class DataStorage
    {
        [Serializable]
        private class SaveData
        {
            public int PermanentCoins;
            public int PermanentGems;

            public int HighScore
            {
                get { return _highscore; }

                set { _highscore = Mathf.Max(value, _highscore); }
            }
            private int _highscore;

            public SaveData()
            {
                PermanentCoins = 0;
                PermanentGems = 0;
                HighScore = 0;
            }

            private SaveData(int score, int coins, int gems)
            {
                HighScore = score;
                PermanentCoins = coins;
                PermanentGems = gems;
            }
        }
    }
}
