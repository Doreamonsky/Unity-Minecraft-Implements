using System.Collections.Generic;
using UnityEngine;

namespace ShanghaiWindy.Core
{
    [CreateAssetMenu(fileName = "ModPackageBuildPiplineData", menuName = "ShanghaiWindy/Mods/ModPackageBuildPiplineData")]
    public class ModPackageBuildPiplineData : ScriptableObject
    {
        [Header("The objects in this list should have been set the assetbundle name and variant.")]
        public List<Object> linkedObjects = new List<Object>();

        [Header("It will override the ModPackageData relatedAssets with the assetbundle path of linkedObjects")]
        public ModPackageData linkedModPackage;
    }
}