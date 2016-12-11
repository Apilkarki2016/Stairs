using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;

namespace Stairs.Utils
{
    public partial class DataStorage : Singleton<DataStorage>
    {
        /// <summary>
        /// Readonly access to number of coins the player has.
        /// </summary>
        public int Coins { get { return PersistentData.PermanentCoins; } }

        /// <summary>
        /// Readonly access to number of gems the player has.
        /// </summary>
        public int Gems { get { return PersistentData.PermanentGems; } }

        /// <summary>
        /// Current score of the player.
        /// </summary>
        public int CurrentScore;

        /// <summary>
        /// Readonly access to highest score the players have achieved on this device.
        /// </summary>
        public int HighScore { get { return PersistentData.HighScore; } }

        /// <summary>
        /// Path to savefile.
        /// </summary>
        private string SaveFile { get { return Path.Combine(Application.persistentDataPath, _saveFile); } }

        /// <summary>
        /// Savefile file-name.
        /// </summary>
        private const string _saveFile = "sav.data";

        /// <summary>
        /// Reference to the saved data container.
        /// </summary>
        private SaveData PersistentData
        {
            get { return _saveData ?? (_saveData = LoadSave()); }
        }

        /// <summary>
        /// Backing field for PersistentData.
        /// </summary>
        private SaveData _saveData = null;

        /// <summary>
        /// Initialize data storage.
        /// </summary>
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            LoadSave();
        }

        /// <summary>
        /// Retrieves the saved data from file or returns new savefile if no save excists.
        /// </summary>
        /// <returns>Retrieved savefile or generated new savefile.</returns>
        private SaveData LoadSave()
        {
            if (!File.Exists(SaveFile)) return new SaveData();

            var data = File.ReadAllBytes(SaveFile);
            var formatter = new DataContractSerializer(typeof(SaveData));
            var stream = new MemoryStream(data);

            return formatter.ReadObject(stream) as SaveData;
        }

        /// <summary>
        /// Write current state of persistent data container to disk.
        /// </summary>
        public void WriteSave()
        {
            var formatter = new DataContractSerializer(typeof(SaveData));
            var stream = new MemoryStream();

            formatter.WriteObject(stream, PersistentData);
            File.WriteAllBytes(SaveFile, stream.GetBuffer());
        }

        /// <summary>
        /// Executed for end-of-playloop actions (=player fall)
        /// </summary>
        public void EndOfPlayLoop()
        {
            PersistentData.HighScore = CurrentScore;
            WriteSave();
        }

        /// <summary>
        /// Start of playloop initialization.
        /// </summary>
        public void InitializeForPlayloop()
        {
            CurrentScore = 0;
        }

        /// <summary>
        /// Adds to current score.
        /// </summary>
        /// <param name="howMuch">Positive integer value to add to score.</param>
        /// <returns></returns>
        public int AddToScore(int howMuch = 1)
        {
            if (howMuch > 0) CurrentScore += howMuch;
            return CurrentScore;;
        }

        /// <summary>
        /// Adds to current coins.
        /// </summary>
        /// <param name="howMuch">Positive integer value to add to coins.</param>
        /// <returns></returns>
        public int AddToCoins(int howMuch = 1)
        {
            if (howMuch > 0) PersistentData.PermanentCoins += howMuch;
            return PersistentData.PermanentCoins;
        }

        /// <summary>
        /// Adds to current gems.
        /// </summary>
        /// <param name="howMuch">Positive integer value to add to gems.</param>
        /// <returns></returns>
        public int AddToGems(int howMuch = 1)
        {
            if (howMuch > 0) PersistentData.PermanentGems += howMuch;
            return PersistentData.PermanentGems;
        }

        /// <summary>
        /// Attempts to pay with given number of coins.
        /// </summary>
        /// <param name="howMany">How many coins to attempt to pay.</param>
        /// <returns>True if player had enough coins to pay with.</returns>
        public bool PayCoins(int howMany)
        {
            if (PersistentData.PermanentCoins < howMany) return false;
            PersistentData.PermanentCoins -= howMany;
            return true;
        }

        /// <summary>
        /// Attempts to pay with given number of gems.
        /// </summary>
        /// <param name="howMany">How many gems to attempt to pay.</param>
        /// <returns>True if player had enough gems to pay with.</returns>
        public bool PayGems(int howMany)
        {
            if (PersistentData.PermanentGems < howMany) return false;
            PersistentData.PermanentGems -= howMany;
            return true;
        }
    }
}