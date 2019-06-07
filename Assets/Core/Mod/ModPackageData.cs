using UnityEngine;
using System.Collections.Generic;

namespace ShanghaiWindy.Core
{
    [CreateAssetMenu(fileName = "ModPackageData", menuName = "ShanghaiWindy/Mods/PackageData")]
    public class ModPackageData : ScriptableObject
    {
        public string modName = "Default Mod";

        public string description = "The Description of the mod";

        public string author = "Your Name";

        public string supportURL = "https://yourWebSite.com";

        public string modVersion = "1.0";

        public string testedGameVersion = "2018.1.1 Vista";

        public string buildTarget = "Default";

        public List<string> relatedAssets = new List<string>();

        [HideInInspector]
        public string installedDirectory = "Assigned-Runtime";
    }
}