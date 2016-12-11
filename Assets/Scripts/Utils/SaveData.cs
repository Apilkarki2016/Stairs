using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stairs.Utils
{
    public partial class DataStorage
    {
        /// <summary>
        /// Serializable container class for data to be written on disk between play sessions.
        /// </summary>
        [Serializable]
        private class SaveData
        {
            /// <summary>
            /// Number of coins the player has.
            /// </summary>
            public int PermanentCoins;

            /// <summary>
            /// Number of gems the player has.
            /// </summary>
            public int PermanentGems;

            /// <summary>
            /// Highscroe the player has achieved.
            /// </summary>
            public int HighScore
            {
                get { return _highscore; }

                set { _highscore = Mathf.Max(value, _highscore); }
            }

            /// <summary>
            /// backing field for highscore.
            /// </summary>
            private int _highscore;

            /// <summary>
            /// Constructor.
            /// </summary>
            public SaveData()
            {
                PermanentCoins = 0;
                PermanentGems = 0;
                HighScore = 0;
            }

            /// <summary>
            /// Alternate constructor with custom parameters..
            /// </summary>
            /// <param name="score">Initial highscore.</param>
            /// <param name="coins">Initial number of coins.</param>
            /// <param name="gems">Initial number of gems.</param>
            private SaveData(int score, int coins, int gems)
            {
                HighScore = score;
                PermanentCoins = coins;
                PermanentGems = gems;
            }
        }
    }
}
