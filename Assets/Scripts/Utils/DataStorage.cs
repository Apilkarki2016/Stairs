using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;

namespace Stairs.Utils
{
    public partial class DataStorage : Singleton<DataStorage>
    {
        public int StepsCurrent;
        public int HighScore { get { return PersistentData.HighScore; } }

        private string SaveFile { get { return Path.Combine(Application.persistentDataPath, _saveFile); } }
        private const string _saveFile = "sav.data";

        private SaveData PersistentData
        {
            get { return _saveData ?? (_saveData = LoadSave()); }
        }
        private SaveData _saveData = null;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            LoadSave();
        }

        private SaveData LoadSave()
        {
            if (!File.Exists(SaveFile)) return new SaveData();

            var data = File.ReadAllBytes(SaveFile);
            var formatter = new DataContractSerializer(typeof(SaveData));
            var stream = new MemoryStream(data);

            return formatter.ReadObject(stream) as SaveData;
        }

        public void WriteSave()
        {
            var formatter = new DataContractSerializer(typeof(SaveData));
            var stream = new MemoryStream();

            formatter.WriteObject(stream, PersistentData);
            File.WriteAllBytes(SaveFile, stream.GetBuffer());
        }

        public void EndOfPlayLoop()
        {
            PersistentData.HighScore = StepsCurrent;
            WriteSave();
        }

        public void InitializeForPlayloop()
        {
            StepsCurrent = 0;
        }

        public int AddToSteps(int howMuch = 1)
        {
            if (howMuch > 0) StepsCurrent += howMuch;
            return StepsCurrent;;
        }

        public int AddToCoins(int howMuch = 1)
        {
            if (howMuch > 0) PersistentData.PermanentCoins += howMuch;
            return PersistentData.PermanentCoins;
        }

        public int AddToGems(int howMuch = 1)
        {
            if (howMuch > 0) PersistentData.PermanentGems += howMuch;
            return PersistentData.PermanentGems;
        }

        public bool PayCoins(int howMany)
        {
            if (PersistentData.PermanentCoins < howMany) return false;
            PersistentData.PermanentCoins -= howMany;
            return true;
        }

        public bool PayGems(int howMany)
        {
            if (PersistentData.PermanentGems < howMany) return false;
            PersistentData.PermanentGems -= howMany;
            return true;
        }
    }
}