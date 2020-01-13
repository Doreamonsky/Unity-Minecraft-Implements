using UnityEngine;


namespace MC.Core
{
    public class AchievementManager : MonoBehaviour
    {
        public static AchievementData Data;

        private void Awake()
        {
            LoadData();
        }

        private void LoadData()
        {
            Data = ScriptableObject.CreateInstance<AchievementData>();
            GeneralStorageSystem.LoadFile(Data, "Achievements");

            Data.OnValueChanged += () =>
            {
                SaveData();
            };
        }

        private void SaveData()
        {
            GeneralStorageSystem.SaveFile(Data, "Achievements");
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }
    }
}
