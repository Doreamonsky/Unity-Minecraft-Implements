using System.Collections.Generic;
using UnityEngine;

namespace ShanghaiWindy.Core
{
    [System.Serializable]
    public class ModActivationData
    {
        public static List<ModPackageData> modPackagesList = new List<ModPackageData>();

        public List<string> ModsIgnoreList = new List<string>();

        public void OnSave()
        {
            PlayerPrefs.SetString("ModActivationData", JsonUtility.ToJson(this));
        }

        public void OnLoad()
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("ModActivationData"), this);
        }

        public static ModPackageData GetModPackage(string modInstallDir)
        {
            return modPackagesList.Find(val =>
            {
                return val.installedDirectory == modInstallDir;
            });
        }
    }
}