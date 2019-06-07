using ICSharpCode.SharpZipLib.Zip;
//using WaroftanksClientSDK;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace ShanghaiWindy.Core
{
    public class AssetBundleManager : MonoBehaviour
    {
        public static List<string> modDirs = new List<string>();

        public static List<string> modDepends = new List<string>();

        public static ModActivationData activationData = new ModActivationData();

        public static void Init()
        {
            activationData.OnLoad();

            //TODO: Mod Manager
            modDirs = new List<string>();

            DirectoryInfo dir = null;

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    dir = new DirectoryInfo(Application.dataPath + "/../mods/");
                    break;
                case RuntimePlatform.WindowsPlayer:
                    dir = new DirectoryInfo(Application.dataPath + "/../mods/");
                    break;
                case RuntimePlatform.Android:
                    dir = new DirectoryInfo(Application.persistentDataPath + "/mods/");
                    break;
            }

            if (dir != null)
            {
                if (!dir.Exists)
                {
                    dir.Create();
                }

                var scriptableMods = new GameObject("ScriptableModManager", typeof(ScriptableModManager)).GetComponent<ScriptableModManager>();

                //Unzip Mod packs
                var installDir = new DirectoryInfo(dir.FullName + "Installs");

                if (!installDir.Exists)
                {
                    installDir.Create();
                }

                var zips = installDir.GetFiles("*.modpack");

                var fastZip = new FastZip();

                foreach (var zip in zips)
                {
                    var packDir = new DirectoryInfo(dir.FullName + Path.GetFileNameWithoutExtension(zip.Name));

                    if (!packDir.Exists)
                    {
                        packDir.Create();
                    }

                    fastZip.ExtractZip(zip.FullName, packDir.FullName, null);


                    var detectInstalled = new FileInfo(zip.Directory.FullName + "/" + Path.GetFileNameWithoutExtension(zip.Name) + ".installed");

                    if (detectInstalled.Exists)
                    {
                        detectInstalled.Delete();
                    }

                    zip.MoveTo(zip.Directory.FullName + "/" + Path.GetFileNameWithoutExtension(zip.Name) + ".installed");
                }

                foreach (var subDir in dir.GetDirectories())
                {
                    if (activationData.ModsIgnoreList.Contains(subDir.FullName + "/"))
                    {
                        continue;
                    }

                    modDirs.Add(subDir.FullName + "/");
                    //Filter Files
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        if (!subDir.Name.Contains("Android"))
                        {
                            continue;
                        }
                    }

                    if (Application.platform == RuntimePlatform.WindowsPlayer)
                    {
                        if (!subDir.Name.Contains("Windows"))
                        {
                            continue;
                        }
                    }

                    if (subDir.Name.Contains("("))
                    {
                        continue;
                    }

                    //Get Depends

                    foreach (var dependFile in subDir.GetFiles("packages"))
                    {
                        modDepends.Add(dependFile.FullName);
                    }

                    foreach (var dll in subDir.GetFiles("*.dll"))
                    {
                        scriptableMods.dllDirs.Add(dll.FullName);
                    }
                }

                scriptableMods.Mount();
            }

        }
    }
}