using ICSharpCode.SharpZipLib.Zip;
using System.Collections;
//using WaroftanksClientSDK;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace ShanghaiWindy.Core
{
    public class AssetBundleManager : MonoBehaviour
    {
        public static List<string> modDirs = new List<string>();

        public static ModActivationData activationData = new ModActivationData();

        public static Queue<AssetRequestTask> assetTaskQueue = new Queue<AssetRequestTask>();

        private static bool hasTaskToFinish = false;

        public static Dictionary<string, AssetBundle> LoadedAssets = new Dictionary<string, AssetBundle>();

        private static List<AssetBundleManifest> assetBundleManifestList = new List<AssetBundleManifest>();

        public static MonoBehaviour MonoActiveObject;

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


                    foreach (var dll in subDir.GetFiles("*.dll"))
                    {
                        scriptableMods.dllDirs.Add(dll.FullName);
                    }

                    foreach (var modInfoFile in subDir.GetFiles("*.modpackdata"))
                    {
                        var assetRequest = new AssetRequestTask()
                        {
                            onAssetLoaded = (val) =>
                            {
                                var modPackageData = val as ModPackageData;

                                if (modPackageData != null)
                                {
                                    modPackageData.installedDirectory = subDir.Name;
                                    ModActivationData.modPackagesList.Add(modPackageData);
                                }
                            }
                        };

                        assetRequest.SetAssetBundleName(modInfoFile.Name);
                        LoadAssetFromAssetBundle(assetRequest);
                    }

                }
                scriptableMods.Mount();
            }

        }

        public static void LoadAssetFromAssetBundle(AssetRequestTask assetRequestTask)
        {
            assetTaskQueue.Enqueue(assetRequestTask);
            hasTaskToFinish = true;
        }

        public static IEnumerator AssetBundleLoop()
        {
            while (true)
            {
                while (hasTaskToFinish == false)
                {
                    yield return new WaitForEndOfFrame();
                }
                //处理Task
                while (assetTaskQueue.Count > 0)
                {
                    AssetRequestTask current = assetTaskQueue.Dequeue();
                    //AssetBundle已经被读取 
                    if (LoadedAssets.ContainsKey(current.GetABName()))
                    {
                        if (LoadedAssets[current.GetABName()] == null)
                        {
                            current.onAssetLoaded(null);
                            Debug.LogError("Null Asset Request Critical!");
                            continue;
                        }

                        string assetPath = LoadedAssets[current.GetABName()].GetAllAssetNames()[0];

                        AssetBundleRequest abRequest = LoadedAssets[current.GetABName()].LoadAssetAsync(assetPath);

                        yield return abRequest;

                        current.onAssetLoaded(abRequest.asset);
                    }
                    //AssetBundle尚未读取
                    else
                    {
                        //依赖包 Loop
                        for (var i = 0; i < assetBundleManifestList.Count; i++)
                        {
                            var DependenciesInfo = assetBundleManifestList[i].GetAllDependencies(current.GetABName());

                            for (var j = 0; j < DependenciesInfo.Length; j++)
                            {
                                if (LoadedAssets.ContainsKey(DependenciesInfo[j]))
                                {
                                    continue;
                                }

                                var isDependenceAssetLoaded = false;

                                MonoActiveObject.StartCoroutine(LoadAsset(DependenciesInfo[j],
                                    () => { isDependenceAssetLoaded = true; }));

                                while (!isDependenceAssetLoaded)
                                {
                                    yield return new WaitForEndOfFrame();
                                }
                            }
                        }
                        //主AB
                        bool isMainLoaded = false;

                        MonoActiveObject.StartCoroutine(LoadAsset(current.GetABName(), () =>
                        {
                            isMainLoaded = true;
                        }));

                        while (!isMainLoaded)
                        {
                            yield return new WaitForEndOfFrame();
                        }

                        //添加回队列 等待队列回调
                        assetTaskQueue.Enqueue(current);

                        yield return new WaitForEndOfFrame();
                    }
                }
                hasTaskToFinish = false;
            }
        }

        private static IEnumerator LoadAsset(string assetBundleName, System.Action onFinish)
        {
            //Debug.LogFormat("Loading:{0}", assetBundleName);

            if (LoadedAssets.ContainsKey(assetBundleName))
            {
                onFinish?.Invoke();
                yield break;
            }

            WWW www = new WWW(GetABLoadPath(assetBundleName));

            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                LoadedAssets.Add(assetBundleName, www.assetBundle);
                onFinish?.Invoke();
            }
            else
            {
                LoadedAssets.Add(assetBundleName, null);
                onFinish?.Invoke();
            }
        }

        private static (string, string) GetAssetbundleLoadPath(bool isApkBundle)
        {
            string path = "";

            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    path = Application.dataPath + "!/assets/packages/";
                    break;
                case RuntimePlatform.WindowsPlayer:
                    path = Application.dataPath + "/../packages/";
                    break;
                case RuntimePlatform.WSAPlayerARM:
                    path = Application.streamingAssetsPath + "/packages/";
                    break;
                case RuntimePlatform.WSAPlayerX86:
                    path = Application.streamingAssetsPath + "/packages/";
                    break;
                case RuntimePlatform.WSAPlayerX64:
                    path = Application.streamingAssetsPath + "/packages/";
                    break;
                case RuntimePlatform.WindowsEditor:
                    path = Application.streamingAssetsPath + "/packages/";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    path = Application.streamingAssetsPath + "/packages/";
                    break;
                case RuntimePlatform.OSXPlayer:
                    path = Application.dataPath + "/StreamingAssets/packages/";
                    break;
                case RuntimePlatform.OSXEditor:
                    path = Application.streamingAssetsPath + "/packages/";
                    break;
            }

            return (path, isApkBundle ? "jar:file://" : "file://");
        }


        public static string GetABLoadPath(string abName)
        {
            var gameFolder = GetAssetbundleLoadPath(Application.platform == RuntimePlatform.Android).Item1;

            FileInfo gameRes;

            gameRes = new FileInfo(gameFolder + abName);

            foreach (var modFolder in modDirs)
            {
                var modRes = new FileInfo(modFolder + "/" + abName);

                if (modRes.Exists)
                {
                    return GetAssetbundleLoadPath(false).Item2 + modRes.FullName;
                }
            }

            return GetAssetbundleLoadPath(Application.platform == RuntimePlatform.Android).Item2 + gameRes.FullName;
        }
    }
}