using MC.Core.Interface;
using System.IO;
using UnityEngine;

namespace MC.Core
{
    public class GeneralStorageSystem
    {
        private static string GetStorageFile(string fileName)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return $"{Application.persistentDataPath}/saves/{fileName}.json";
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return $"{Application.persistentDataPath}/saves/{fileName}.json";
            }

            return $"{Application.dataPath}/../saves/{fileName}.json";
        }

        private static string GetStorageFolder()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return $"{Application.persistentDataPath}/saves/";
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return $"{Application.persistentDataPath}/saves/";
            }

            return $"{Application.dataPath}/../saves/";
        }

        public static bool HasFile(string savingName)
        {
            var saveFile = new FileInfo(GetStorageFile(savingName));
            return saveFile.Exists;
        }

        public static bool SaveFile(ScriptableObject scriptableObject, string savingName)
        {
            try
            {
                var fileSave = scriptableObject as IFileSave;

                if (fileSave != null)
                {
                    fileSave.OnSave();
                }

                var json = JsonUtility.ToJson(scriptableObject);

                var fileStream = new FileStream(GetStorageFile(savingName), FileMode.Create);
                var streamWriter = new StreamWriter(fileStream);

                streamWriter.Write(json);

                streamWriter.Flush();
                fileStream.Flush();

                streamWriter.Close();
                fileStream.Close();

                return true;
            }
            catch
            {
                return false;
            }

        }

        public static bool LoadFile(ScriptableObject scriptableObject, string savingName)
        {
            try
            {
                var saveFile = new FileInfo(GetStorageFile(savingName));

                var dic = new DirectoryInfo(saveFile.DirectoryName);

                if (!dic.Exists)
                {
                    dic.Create();
                }

                if (saveFile.Exists)
                {
                    var fileStream = new FileStream(GetStorageFile(savingName), FileMode.Open);
                    var steamReader = new StreamReader(fileStream);

                    var json = steamReader.ReadToEnd();
                    JsonUtility.FromJsonOverwrite(json, scriptableObject);

                    steamReader.Close();
                    fileStream.Close();
                }

                var fileLoad = scriptableObject as IFileLoad;

                if (fileLoad != null)
                {
                    fileLoad.OnLoad();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void DeleteFile(string savingName)
        {
            var saveFile = new FileInfo(GetStorageFile(savingName));

            if (saveFile.Exists)
            {
                saveFile.Delete();
            }
            else
            {
                Debug.Log("Can not file file");
            }
        }

        public static void DeleteFolder()
        {
            var folder = new DirectoryInfo(GetStorageFolder());
            folder.Delete(true);
        }
    }
}
